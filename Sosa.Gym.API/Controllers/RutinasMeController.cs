using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignada;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignadaDetalle;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/me/rutinas")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    public class RutinasMeController : ControllerBase
    {
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdStr, out userId);
        }


        [HttpGet]
        public async Task<IActionResult> GetMyRutinas(
            [FromServices] IGetRutinasAsignadasQuery query)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(ResponseApiService.Response(401, "Token inválido"));

            var result = await query.Execute(userId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{rutinaId:int}")]
        public async Task<IActionResult> GetMyRutinaDetalle(
            [FromRoute] int rutinaId,
            [FromServices] IGetRutinaAsignadaDetalleQuery query)
        {
            if (rutinaId <= 0)
                return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));

            if (!TryGetUserId(out var userId))
                return Unauthorized(ResponseApiService.Response(401, "Token inválido"));

            var result = await query.Execute(rutinaId, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
