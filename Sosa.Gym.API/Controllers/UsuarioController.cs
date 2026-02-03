using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/usuarios")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllUsuariosQuery getAllUsuariosQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await getAllUsuariosQuery.Execute(pageNumber, pageSize);

            return StatusCode(
                StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data));
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int userId,
            [FromServices] IGetUsuarioByIdQuery getUsuarioByIdQuery)
        {
            if (userId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Id inválido"));
            }

            var result = await getUsuarioByIdQuery.Execute(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("dni/{dni:long}")]
        public async Task<IActionResult> GetByDni(
            [FromRoute] long dni,
            [FromServices] IGetUsuarioByDniQuery getUsuarioByDniQuery)
        {
            if (dni <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "DNI inválido"));
            }

            var result = await getUsuarioByDniQuery.Execute(dni);
            return StatusCode(result.StatusCode, result);
        }
    }
}
