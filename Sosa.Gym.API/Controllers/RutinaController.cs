using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Rutina.CreateRutina;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

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
    }
}
