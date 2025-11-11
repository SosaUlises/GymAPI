using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/usuario")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class UsuarioController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllUsuariosQuery getAllUsuariosQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await getAllUsuariosQuery.Execute(pageNumber, pageSize);

            if (!data.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data));

        }

        [AllowAnonymous]
        [HttpGet("getById/{userId}")]
        public async Task<IActionResult> GetById(
            int userId,
            [FromServices] IGetUsuarioByIdQuery getUsuarioByIdQuery)
        {

            if (userId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await getUsuarioByIdQuery.Execute(userId);

            return StatusCode(data.StatusCode, data);
        }

        [AllowAnonymous]
        [HttpGet("getByDni/{dni}")]
        public async Task<IActionResult> GetByDni(
            long dni,
            [FromServices] IGetUsuarioByDniQuery getUsuarioByDniQuery)
        {

            if (dni == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await getUsuarioByDniQuery.Execute(dni);

            return StatusCode(data.StatusCode, data);
        }



    }
}
