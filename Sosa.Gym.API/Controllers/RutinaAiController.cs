using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService;

namespace Sosa.Gym.API.Controllers
{
    [ApiController]
    [Route("api/v1/ai/rutinas")]
    public class RutinaAiController : ControllerBase
    {
        [Authorize(Roles = "Administrador,Entrenador")]
        [HttpPost("preview")]
        public async Task<IActionResult> Preview(
            [FromBody] GenerarRutinaPreviewRequest request,
            [FromServices] IGenerarRutinaPreviewService service)
        {
            var result = await service.Execute(request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
