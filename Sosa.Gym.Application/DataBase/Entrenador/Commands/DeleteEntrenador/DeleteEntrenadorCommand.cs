using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.DeleteEntrenador
{
    public class DeleteEntrenadorCommand : IDeleteEntrenadorCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;

        public DeleteEntrenadorCommand(UserManager<UsuarioEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseResponseModel> Execute(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Entrenador"))
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "El usuario no es Entrenador");

            // Seguridad: no permitir borrar Admin 
            if (roles.Contains("Administrador"))
                return ResponseApiService.Response(StatusCodes.Status409Conflict, "No se puede eliminar un Administrador");

            var delRes = await _userManager.DeleteAsync(user);
            if (!delRes.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, delRes.Errors, "Error al eliminar entrenador");

            return ResponseApiService.Response(StatusCodes.Status200OK, "Entrenador eliminado correctamente");
        }
    }
}
