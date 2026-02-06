using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(Roles = "Administrador,Entrenador")]
    public class DiaRutinaController : ControllerBase
    {

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

            var result = await command.Execute(rutinaId, model);
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

            var result = await command.Execute(diaRutinaId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
