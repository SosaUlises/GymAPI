using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll;
using Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotaByCliente;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CuotaController : ControllerBase
    {
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out userId);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost("clientes/{clienteId:int}/cuotas")]
        public async Task<IActionResult> CreateCuota(
            [FromRoute] int clienteId,
            [FromBody] CreateCuotaModel model,
            [FromServices] ICreateCuotaCommand command,
            [FromServices] IValidator<CreateCuotaModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var result = await command.Execute(clienteId, model);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost("cuotas/generacion")]
        public async Task<IActionResult> GenerarCuotas(
            [FromBody] GenerarCuotasModel model,
            [FromServices] IGenerarCuotasCommand command,
            [FromServices] IValidator<GenerarCuotasModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var result = await command.Execute(model);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador,Entrenador")]
        [HttpPost("cuotas/{cuotaId:int}/registrar-pago")]
        public async Task<IActionResult> RegistrarPago(
         [FromRoute] int cuotaId,
         [FromBody] PagarCuotaModel model,
         [FromServices] IPagarCuotaCommand command,
         [FromServices] IValidator<PagarCuotaModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));

            if (!TryGetUserId(out var userId))
                return Unauthorized(ResponseApiService.Response(StatusCodes.Status401Unauthorized, "Token inválido"));

            var result = await command.Execute(cuotaId, model, userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("cuotas/cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByClienteId(
            [FromRoute] int clienteId,
            [FromServices] IGetCuotaByClienteQuery query)
        {
            if (clienteId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "ClienteId inválido"));
            }

            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var esAdmin = User.IsInRole("Administrador");
            var result = await query.Execute(clienteId, userId, esAdmin);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet("cuotas/estado/{estado}")]
        public async Task<IActionResult> GetByEstado(
            [FromRoute] string estado,
            [FromServices] IGetCuotasByEstadoQuery query)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            if (!Enum.TryParse<EstadoCuota>(estado, ignoreCase: true, out var estadoEnum))
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Estado inválido"));
            }

            var esAdmin = User.IsInRole("Administrador");


            var result = await query.Execute(estadoEnum, userId, esAdmin);
            return StatusCode(result.StatusCode, result);
        }
    }
}
