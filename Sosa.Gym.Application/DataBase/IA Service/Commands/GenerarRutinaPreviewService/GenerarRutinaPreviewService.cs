using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sosa.Gym.Application.DataBase.IA_Service.GenerarRutinaPreviewService;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
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
            if (request.DiasPorSemana < 1 || request.DiasPorSemana > 7)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "DiasPorSemana inválido");

            var apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "OpenAI ApiKey no configurada");

            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

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


            var user = """
            Genera una rutina semanal. Formato JSON exacto:
            {
              "nombre": "string",
              "descripcion": "string",
              "dias": [
                {
                  "nombreDia": "string",
                  "ejercicios": [
                    {
                      "nombre": "string",
                      "series": 3,
                      "repeticiones": 10,
                      "pesoUtilizado": 0
                    }
                  ]
                }
              ]
            }
            """;

                        user =
                        $"""
            Objetivo: {request.Objetivo}
            Días por semana: {request.DiasPorSemana}
            Nivel: {request.Nivel}
            Duración por sesión (min): {request.DuracionMinutos}
            Equipamiento: {request.Equipamiento ?? "no especificado"}
            Restricciones: {request.Restricciones ?? "ninguna"}

            """ + user;


            // Chat Completions API

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
            var resp = await _http.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode)
            {
                var errorBody = await resp.Content.ReadAsStringAsync();
                return ResponseApiService.Response(
                    StatusCodes.Status502BadGateway,
                    new { OpenAIStatus = (int)resp.StatusCode, Error = errorBody },
                    "Error llamando a OpenAI");
            }

            var body = await resp.Content.ReadAsStringAsync();

            // Parse: sacamos el content del assistant y lo parseamos como RutinaPreviewResponse
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

                var rutina = JsonSerializer.Deserialize<RutinaPreviewResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (rutina == null || rutina.Dias.Count == 0)
                    return ResponseApiService.Response(StatusCodes.Status502BadGateway, "JSON inválido o incompleto (OpenAI)");

                // FORZAR peso=0 (blindaje)
                foreach (var d in rutina.Dias)
                    foreach (var e in d.Ejercicios)
                        e.PesoUtilizado = 0;

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
    }
}
