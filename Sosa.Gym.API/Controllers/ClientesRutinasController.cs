using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinasAsignadasAdminByCliente;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/clientes")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class ClientesRutinasController : ControllerBase
    {

        [HttpGet("{clienteId:int}/rutinas")]
        public async Task<IActionResult> GetRutinasAsignadasDeCliente(
            [FromRoute] int clienteId,
            [FromServices] IGetRutinasAsignadasAdminByClienteIdQuery query)
        {
            if (clienteId <= 0)
                return BadRequest(ResponseApiService.Response(400, "ClienteId inválido"));

            var result = await query.Execute(clienteId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
