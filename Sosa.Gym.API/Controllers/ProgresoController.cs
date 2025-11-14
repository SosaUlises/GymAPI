using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/progreso")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class ProgresoController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
                 [FromBody] CreateProgresoModel model,
                 [FromServices] ICreateProgresoCommand createProgresoCommand,
                 [FromServices] IValidator<CreateProgresoModel> validator
                 )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var progreso = await createProgresoCommand.Execute(model);

            return StatusCode(progreso.StatusCode, progreso);

        }
    }
}
