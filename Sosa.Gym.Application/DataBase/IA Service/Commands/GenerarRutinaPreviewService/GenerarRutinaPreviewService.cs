using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService
{
    public class GenerarRutinaPreviewService : IGenerarRutinaPreviewService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public GenerarRutinaPreviewService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<BaseResponseModel> Execute(GenerarRutinaPreviewRequest request)
        {
            // Validaciones básicas input
            if (request.DiasPorSemana < 1 || request.DiasPorSemana > 7)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "DiasPorSemana inválido (1..7)");

            if (string.IsNullOrWhiteSpace(request.Objetivo))
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Objetivo es requerido");

            if (string.IsNullOrWhiteSpace(request.Nivel))
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Nivel es requerido");

            if (request.DuracionMinutos <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "DuracionMinutos inválido");

            var apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "OpenAI ApiKey no configurada");

            var system = """
                Eres un entrenador personal profesional.
                Devuelve ÚNICAMENTE JSON válido.
                No uses markdown.
                No agregues texto fuera del JSON.
                No inventes campos fuera del schema.
                Todos los ejercicios deben tener:
                - series (int > 0)
                - repeticiones (int > 0)
                - pesoUtilizado = 0
                """;

            var user = @$"
            Objetivo: {request.Objetivo}
            Días por semana: {request.DiasPorSemana}
            Nivel: {request.Nivel}
            Duración por sesión (min): {request.DuracionMinutos}
            Equipamiento: {request.Equipamiento ?? "no especificado"}
            Restricciones: {request.Restricciones ?? "ninguna"}

            Genera una rutina semanal. Formato JSON exacto:
            {{
              ""nombre"": ""string"",
              ""descripcion"": ""string"",
              ""dias"": [
                {{
                  ""nombreDia"": ""string"",
                  ""ejercicios"": [
                    {{
                      ""nombre"": ""string"",
                      ""series"": 3,
                      ""repeticiones"": 10,
                      ""pesoUtilizado"": 0
                    }}
                  ]
                }}
              ]
            }}
            ";


            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new { role = "system", content = system },
                    new { role = "user", content = user }
                },
                temperature = 0.4,
                max_tokens = 700
            };

            var json = JsonSerializer.Serialize(payload);

            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);

            var body = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {

                return ResponseApiService.Response(
                    StatusCodes.Status502BadGateway,
                    new { OpenAIStatus = (int)resp.StatusCode, Error = body },
                    "Error llamando a OpenAI");
            }

            try
            {
                using var doc = JsonDocument.Parse(body);
                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(content))
                    return ResponseApiService.Response(StatusCodes.Status502BadGateway, "Respuesta vacía de OpenAI");

                var jsonOnly = ExtractJsonObject(RemoveCodeFences(content));

                var rutina = JsonSerializer.Deserialize<RutinaPreviewResponse>(
                    jsonOnly,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (rutina == null || rutina.Dias == null || rutina.Dias.Count == 0)
                    return ResponseApiService.Response(StatusCodes.Status502BadGateway, "JSON inválido o incompleto (OpenAI)");

                // Validaciones de negocio mínimas
                if (rutina.Dias.Count > 7)
                    return ResponseApiService.Response(StatusCodes.Status502BadGateway, "OpenAI devolvió más de 7 días");

                if (rutina.Dias.Any(d => string.IsNullOrWhiteSpace(d.NombreDia) || d.Ejercicios == null || d.Ejercicios.Count == 0))
                    return ResponseApiService.Response(StatusCodes.Status502BadGateway, "Rutina incompleta (días sin ejercicios)");

                foreach (var d in rutina.Dias)
                {
                    foreach (var e in d.Ejercicios)
                    {
                        if (string.IsNullOrWhiteSpace(e.Nombre) || e.Series <= 0 || e.Repeticiones <= 0)
                            return ResponseApiService.Response(StatusCodes.Status502BadGateway, "Ejercicios inválidos (series/reps/nombre)");

                        // Blindaje peso = 0
                        e.PesoUtilizado = 0;
                    }
                }

                return ResponseApiService.Response(StatusCodes.Status200OK, rutina);
            }
            catch (Exception ex)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status502BadGateway,
                    new { Error = ex.Message, Raw = body },
                    "No se pudo parsear el JSON generado");
            }
        }

        private static string RemoveCodeFences(string s)
        {
            var t = s.Trim();
            if (t.StartsWith("```"))
            {
                t = t.Replace("```json", "", StringComparison.OrdinalIgnoreCase)
                     .Replace("```", "");
                t = t.Trim();
            }
            return t;
        }

        private static string ExtractJsonObject(string s)
        {
            var start = s.IndexOf('{');
            var end = s.LastIndexOf('}');
            if (start < 0 || end < 0 || end <= start) return s;
            return s.Substring(start, end - start + 1);
        }
    }
}
