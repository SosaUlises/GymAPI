using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente
{
    public class UpdateClienteCommand : IUpdateClienteCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateClienteCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper,
            IDataBaseService dataBaseService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dataBaseService = dataBaseService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseRespondeModel> Execute(UpdateClienteModel model, int userId)
        {
            // Obtener rol del usuario logueado
            var httpUser = _httpContextAccessor.HttpContext.User;
            var roles = httpUser.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            bool esAdmin = roles.Contains("Administrador");

            // Validar propiedad SOLO si es Cliente
            if (!esAdmin && model.Id != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes acceder a datos de otro usuario");
            }

            var usuario = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (usuario == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Usuario no encontrado");

            var existeEmail = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.Id != usuario.Id);

            if (existeEmail != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el email {model.Email}");

            var existeDni = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Dni == model.Dni && x.Id != usuario.Id);

            if (existeDni != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el DNI {model.Dni}");

            _mapper.Map(model, usuario);
            var result = await _userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    "Error al modificar el usuario");

            // Roles (solo admin puede tocar roles)
            if (esAdmin)
            {
                var currentRoles = await _userManager.GetRolesAsync(usuario);

                if (!currentRoles.Contains(model.Rol))
                {
                    await _userManager.RemoveFromRolesAsync(usuario, currentRoles);
                    await _userManager.AddToRoleAsync(usuario, model.Rol);
                }
            }

            // Actualizar Cliente
            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (cliente == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            _mapper.Map(model, cliente);

            _dataBaseService.Clientes.Update(cliente);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK,
                "Cliente actualizado correctamente");
        }
    }

}
