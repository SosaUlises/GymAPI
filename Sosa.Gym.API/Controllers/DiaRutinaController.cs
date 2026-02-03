using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    public class DiaRutinaController : ControllerBase
    {
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out userId);
        }


        [HttpPost("rutinas/{rutinaId:int}/dias")]
        public async Task<IActionResult> CreateDiaRutina(
            [FromRoute] int rutinaId,
            [FromBody] CreateDiaRutinaModel model,
            [FromServices] ICreateDiaRutinaCommand command,
            [FromServices] IValidator<CreateDiaRutinaModel> validator)
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

            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await command.Execute(rutinaId, model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("rutinas/{rutinaId:int}/dias")]
        public async Task<IActionResult> GetByRutinaId(
            [FromRoute] int rutinaId,
            [FromServices] IGetDiaRutinaQuery query)
        {
            if (rutinaId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "RutinaId inválido"));
            }

            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await query.Execute(rutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }

    
        [HttpDelete("dias-rutina/{diaRutinaId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int diaRutinaId,
            [FromServices] IDeleteDiaRutinaCommand command)
        {
            if (diaRutinaId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "DiaRutinaId inválido"));
            }

            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await command.Execute(diaRutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
