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
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente
{
    public class UpdateClienteCommand : IUpdateClienteCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        public UpdateClienteCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper,
            IDataBaseService dataBaseService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseRespondeModel> Execute(UpdateClienteModel model)
        {

            var usuario = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (usuario == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Usuario no encontrado");

            var existeEmail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            var existeDni = await _userManager.Users.FirstOrDefaultAsync(x => x.Dni == model.Dni);

            if (existeEmail != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el email {model.Email}");

            if (existeDni != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el DNI {model.Dni}");

            _mapper.Map(model, usuario);
            var result = await _userManager.UpdateAsync(usuario);
            if (!result.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Error al modificar el usuario");


            var currentRoles = await _userManager.GetRolesAsync(usuario);
            if (!currentRoles.Contains(model.Rol))
            {
                await _userManager.RemoveFromRolesAsync(usuario, currentRoles);
                await _userManager.AddToRoleAsync(usuario, model.Rol);
            }


            var cliente = await _dataBaseService.Clientes
                .FirstOrDefaultAsync(c => c.Id == model.Id);

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
