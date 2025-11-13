using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/diaRutina")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class DiaRutinaController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
                [FromBody] CreateDiaRutinaModel model,
                [FromServices] ICreateDiaRutinaCommand createDiaRutinaCommand,
                [FromServices] IValidator<CreateDiaRutinaModel> validator
                )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var diaRutina = await createDiaRutinaCommand.Execute(model);

            return StatusCode(diaRutina.StatusCode, diaRutina);
        }

        [AllowAnonymous]
        [HttpDelete("delete/{diaRutinaId}")]
        public async Task<IActionResult> Delete(
                int diaRutinaId,
                [FromServices] IDeleteDiaRutinaCommand deleteDiaRutinaCommand
                )
        {

            if (diaRutinaId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var diaRutina = await deleteDiaRutinaCommand.Execute(diaRutinaId);

            return StatusCode(diaRutina.StatusCode, diaRutina);

        }

        [AllowAnonymous]
        [HttpGet("getDiasRutinaByRutinaId/{rutinaId}")]
        public async Task<IActionResult> GetDiasRutinaByRutinaId(
               int rutinaId,
               [FromServices] IGetDiasRutinaByRutinaIdQuery getDiasRutinaByRutinaIdQuery
               )
        {
            if (rutinaId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var diasRutinas = await getDiasRutinaByRutinaIdQuery.Execute(rutinaId);
            return StatusCode(diasRutinas.StatusCode, diasRutinas);

        }
    }
}
