using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.AsignarRutina;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.DesasignarRutina;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignada;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignadaDetalle;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/rutinas")]
    [ApiController]
    public class RutinaController : ControllerBase
    {

        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out userId);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRutinaModel model,
            [FromServices] ICreateRutinaCommand createRutinaCommand,
            [FromServices] IValidator<CreateRutinaModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var result = await createRutinaCommand.Execute(model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{rutinaId:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int rutinaId,
            [FromBody] UpdateRutinaModel model,
            [FromServices] IUpdateRutinaCommand updateRutinaCommand,
            [FromServices] IValidator<UpdateRutinaModel> validator)
        {
            if (rutinaId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "RutinaId inválido"));
            }

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var result = await updateRutinaCommand.Execute(rutinaId, model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{rutinaId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int rutinaId,
            [FromServices] IDeleteRutinaCommand deleteRutinaCommand)
        {
            if (rutinaId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "RutinaId inválido"));
            }

            var result = await deleteRutinaCommand.Execute(rutinaId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("{rutinaId:int}/asignaciones/{clienteId:int}")]
        public async Task<IActionResult> Asignar(
            [FromRoute] int rutinaId,
            [FromRoute] int clienteId,
            [FromServices] IAsignarRutinaCommand command)
        {
            if (rutinaId <= 0) return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));
            if (clienteId <= 0) return BadRequest(ResponseApiService.Response(400, "ClienteId inválido"));

            var result = await command.Execute(rutinaId, clienteId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{rutinaId:int}/asignaciones/{clienteId:int}")]
        public async Task<IActionResult> Desasignar(
            [FromRoute] int rutinaId,
            [FromRoute] int clienteId,
            [FromServices] IDesasignarRutinaCommand command)
        {
            if (rutinaId <= 0) return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));
            if (clienteId <= 0) return BadRequest(ResponseApiService.Response(400, "ClienteId inválido"));

            var result = await command.Execute(rutinaId, clienteId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Cliente")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyRutinas(
         [FromServices] IGetRutinasAsignadasQuery query)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await query.Execute(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Cliente")]
        [HttpGet("me/{rutinaId:int}")]
        public async Task<IActionResult> GetMyRutinaDetalle(
         [FromRoute] int rutinaId,
         [FromServices] IGetRutinaAsignadaDetalleQuery query)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await query.Execute(rutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
