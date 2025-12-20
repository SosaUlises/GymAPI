using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/progresos")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    [TypeFilter(typeof(ExceptionManager))]
    public class ProgresoController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> Create(
                [FromBody] CreateProgresoModel model,
                [FromServices] ICreateProgresoCommand createProgresoCommand,
                [FromServices] IValidator<CreateProgresoModel> validator)
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

            var result = await createProgresoCommand.Execute(model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("{progresoId:int}")]
        public async Task<IActionResult> Update(
             [FromRoute] int progresoId,
             [FromBody] UpdateProgresoModel model,
             [FromServices] IUpdateProgresoCommand updateProgresoCommand,
             [FromServices] IValidator<UpdateProgresoModel> validator)
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

            var result = await updateProgresoCommand.Execute(progresoId, model, userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMine(
            [FromServices] IGetProgresoQuery query)
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
