using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
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
        [HttpPost("crear")]
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
    }
}
