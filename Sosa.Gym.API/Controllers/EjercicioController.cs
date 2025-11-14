using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Application.Validators.Ejercicio;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/ejercicio")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class EjercicioController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
                 [FromBody] CreateEjercicioModel model,
                 [FromServices] ICreateEjercicioCommand createEjercicioCommand,
                 [FromServices] IValidator<CreateEjercicioModel> validator
                 )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var ejercicio = await createEjercicioCommand.Execute(model);

            return StatusCode(ejercicio.StatusCode, ejercicio);

        }
    }
}
