using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Acceso.Commands.ValidarIngreso;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/accesos")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        [Authorize(Roles = "Administrador,Entrenador")]
        [HttpGet("validar-dni/{dni:long}")]
        public async Task<IActionResult> ValidarDni(
            [FromRoute] long dni,
            [FromServices] IValidarIngresoPorDniCommand command)
        {
            var result = await command.Execute(dni);
            return StatusCode(result.StatusCode, result);
        }
    }
}
