using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/cuotas")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class CuotaController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
              [FromBody] CreateCuotaModel model,
              [FromServices] ICreateCuotaCommand createCuotaCommand,
              [FromServices] IValidator<CreateCuotaModel> validator
              )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var cuota = await createCuotaCommand.Execute(model);

            return StatusCode(cuota.StatusCode, cuota);

        }
    }
}
