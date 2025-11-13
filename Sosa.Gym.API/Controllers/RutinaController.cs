using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Application.Validators.Rutina;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/rutina")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class RutinaController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
                [FromBody] CreateRutinaModel model,
                [FromServices] ICreateRutinaCommand createRutinaCommand,
                [FromServices] IValidator<CreateRutinaModel> validator
                )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var rutina = await createRutinaCommand.Execute(model);

            return StatusCode(rutina.StatusCode, rutina);

        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<IActionResult> Update(
                [FromBody] UpdateRutinaModel model,
                [FromServices] IUpdateRutinaCommand updateRutinaCommand,
                [FromServices] IValidator<UpdateRutinaModel> validator
                )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var rutina = await updateRutinaCommand.Execute(model);

            return StatusCode(rutina.StatusCode, rutina);

        }

        [AllowAnonymous]
        [HttpDelete("delete/{rutinaId}")]
        public async Task<IActionResult> Delete(
               int rutinaId,
               [FromServices] IDeleteRutinaCommand deleteRutinaCommand
               )
        {
            if (rutinaId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var rutina = await deleteRutinaCommand.Execute(rutinaId);
            return StatusCode(rutina.StatusCode, rutina);

        }
    }
}
