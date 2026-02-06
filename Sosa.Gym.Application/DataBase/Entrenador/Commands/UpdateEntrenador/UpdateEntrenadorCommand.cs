using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.UpdateEntrenador
{
    public class UpdateEntrenadorCommand : IUpdateEntrenadorCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;

        public UpdateEntrenadorCommand(UserManager<UsuarioEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponseModel> Execute(int userId, UpdateEntrenadorModel model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Entrenador"))
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "El usuario no es Entrenador");

            var email = model.Email.Trim();

            var existeEmail = await _userManager.Users.AnyAsync(x => x.Email == email && x.Id != user.Id);
            if (existeEmail)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, $"Ya existe un usuario con el email {email}");

            var existeDni = await _userManager.Users.AnyAsync(x => x.Dni == model.Dni && x.Id != user.Id);
            if (existeDni)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, $"Ya existe un usuario con el DNI {model.Dni}");

            user.Nombre = model.Nombre;
            user.Apellido = model.Apellido;
            user.Email = email;
            user.UserName = email;
            user.Dni = model.Dni;

            var updRes = await _userManager.UpdateAsync(user);
            if (!updRes.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, updRes.Errors, "Error al actualizar entrenador");

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passRes = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!passRes.Succeeded)
                    return ResponseApiService.Response(StatusCodes.Status400BadRequest, passRes.Errors, "Error al actualizar contraseña");
            }

            return ResponseApiService.Response(StatusCodes.Status200OK, "Entrenador actualizado correctamente");
        }
    }
}
