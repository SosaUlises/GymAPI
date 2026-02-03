using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/progresos")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    public class ProgresoController : ControllerBase
    {
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out userId);
        }

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

            if (!TryGetUserId(out var userId))
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
            if (progresoId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "ProgresoId inválido"));
            }

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            if (!TryGetUserId(out var userId))
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
            if (!TryGetUserId(out var userId))
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
