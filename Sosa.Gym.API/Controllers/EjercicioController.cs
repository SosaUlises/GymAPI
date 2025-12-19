using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.DeleteEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/ejercicios")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    [TypeFilter(typeof(ExceptionManager))]
    public class EjercicioController : Controller
    {


        [HttpPost("dias-rutina/{diaRutinaId:int}")]
        public async Task<IActionResult> Create(
            [FromRoute] int diaRutinaId,
            [FromBody] CreateEjercicioModel model,
            [FromServices] ICreateEjercicioCommand createEjercicioCommand,
            [FromServices] IValidator<CreateEjercicioModel> validator)
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

            var result = await createEjercicioCommand.Execute(diaRutinaId, model, userId);
            return StatusCode(result.StatusCode, result);
        }



        [HttpPut("{ejercicioId:int}")]
        public async Task<IActionResult> Update(
             [FromRoute] int ejercicioId,
             [FromBody] UpdateEjercicioModel model,
             [FromServices] IUpdateEjercicioCommand updateEjercicioCommand,
             [FromServices] IValidator<UpdateEjercicioModel> validator)
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

            var result = await updateEjercicioCommand.Execute(ejercicioId, model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpDelete("{ejercicioId:int}")]
        public async Task<IActionResult> Delete(
             [FromRoute] int ejercicioId,
             [FromServices] IDeleteEjercicioCommand deleteEjercicioCommand)
        {
            if (ejercicioId <= 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, "Id inválido"));

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized(ResponseApiService.Response(StatusCodes.Status401Unauthorized, "Token inválido"));

            var result = await deleteEjercicioCommand.Execute(ejercicioId, userId);
            return StatusCode(result.StatusCode, result);
        }



        [HttpGet("dias-rutina/{diaRutinaId:int}")]
        public async Task<IActionResult> GetByDiaRutina(
             [FromRoute] int diaRutinaId,
             [FromServices] IGetEjerciciosQuery query)
        {
            if (diaRutinaId <= 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest, "Id inválido"));

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized(ResponseApiService.Response(StatusCodes.Status401Unauthorized, "Token inválido"));

            var result = await query.Execute(diaRutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
