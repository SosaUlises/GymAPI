using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.DeleteUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Application.Validators.Usuario;
using Sosa.Gym.Domain.Entidades.Usuario;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/usuario")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class UsuarioController : ControllerBase
    {


        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Crear(
            [FromBody] CreateUsuarioModel model,
            [FromServices] ICreateUsuarioCommand createUsuarioCommand,
            [FromServices] IValidator<CreateUsuarioModel> validator
            )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var usuario = await createUsuarioCommand.Execute(model);

            return StatusCode(usuario.StatusCode, usuario);
       
        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateUsuarioModel model,
            [FromServices] IUpdateUsuarioCommand updateUsuarioCommand,
            [FromServices] IValidator<UpdateUsuarioModel> validator
            )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(
                    ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var usuarioUpdate = await updateUsuarioCommand.Execute(model);

            return StatusCode(usuarioUpdate.StatusCode,usuarioUpdate); 
        }

        [AllowAnonymous]
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(
            int userId,
            [FromServices] IDeleteUsuarioCommand deleteUsuarioCommand
            )
        {
            if(userId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await deleteUsuarioCommand.Execute(userId);

            if (!data)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                   ResponseApiService.Response(StatusCodes.Status404NotFound));
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK,
                   ResponseApiService.Response(StatusCodes.Status200OK,data));
            }
        }

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

            var data = await getAllUsuariosQuery.Execute(pageNumber,pageSize);

            if (!data.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK,data));

        }

        [AllowAnonymous]
        [HttpGet("getById/{userId}")]
        public async Task<IActionResult> GetById(
            int userId,
            [FromServices] IGetUsuarioByIdQuery getUsuarioByIdQuery)
        {

            if(userId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await getUsuarioByIdQuery.Execute(userId);

            if (data == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                   ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK,
                  ResponseApiService.Response(StatusCodes.Status200OK,data));
        }

       

    }
}
