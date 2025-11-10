using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.DeleteUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
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

            return Ok(ResponseApiService.Response(
                StatusCodes.Status200OK,
                new { usuario.Id, usuario.Email, usuario.Nombre, usuario.Apellido }));
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

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK,usuarioUpdate)); 
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

    }
}
