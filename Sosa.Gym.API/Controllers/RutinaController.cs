using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/rutina")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    [TypeFilter(typeof(ExceptionManager))]
    public class RutinaController : Controller
    {

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

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await createRutinaCommand.Execute(model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("{rutinaId:int}")]
        public async Task<IActionResult> Update(
             [FromRoute] int rutinaId,
             [FromBody] UpdateRutinaModel model,
             [FromServices] IUpdateRutinaCommand updateRutinaCommand,
             [FromServices] IValidator<UpdateRutinaModel> validator)
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

            var result = await updateRutinaCommand.Execute(rutinaId, model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpDelete("{rutinaId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int rutinaId,
            [FromServices] IDeleteRutinaCommand deleteRutinaCommand)
        {
            if (rutinaId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Id inválido"));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await deleteRutinaCommand.Execute(rutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMine(
            [FromServices] IGetRutinaQuery query)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await query.Execute(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
