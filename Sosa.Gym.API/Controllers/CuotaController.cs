using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll;
using Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotaByCliente;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/cuotas")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class CuotaController : Controller
    {

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
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));

            var result = await command.Execute(clienteId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("generar")]
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


        [Authorize(Roles = "Cliente")]
        [HttpPost("{cuotaId:int}/pagar")]
        public async Task<IActionResult> Pagar(
            [FromRoute] int cuotaId,
            [FromBody] PagarCuotaModel model,
            [FromServices] IPagarCuotaCommand command,
            [FromServices] IValidator<PagarCuotaModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await command.Execute(cuotaId, model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet("get-by-clienteId/{clienteId}")]
        public async Task<IActionResult> GetByClienteId(
            int clienteId,
            [FromServices] IGetCuotaByClienteQuery getCuotaByClienteQuery
            )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (clienteId == 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            bool esAdmin = roles.Contains("Administrador");

            var cuotas = await getCuotaByClienteQuery.Execute(clienteId, int.Parse(userId), esAdmin);

            return StatusCode(cuotas.StatusCode, cuotas);

        }

        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet("get-by-estado/{estado}")]
        public async Task<IActionResult> GetByEstado(
           string estado,
           [FromServices] IGetCuotasByEstadoQuery getCuotasByEstadoQuery
           )
        {

            if (estado == "")
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));

            var cuotas = await getCuotasByEstadoQuery.Execute(estado);

            return StatusCode(cuotas.StatusCode, cuotas);

        }
    }
}
