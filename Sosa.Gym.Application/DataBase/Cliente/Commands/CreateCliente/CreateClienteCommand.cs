using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente
{
    public class CreateClienteCommand : ICreateClienteCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateClienteCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper,
            IDataBaseService dataBaseService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(CreateClienteModel model)
        {

            var existeEmail = await _userManager.Users.AnyAsync(x => x.Email == model.Email);
            if (existeEmail)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el email {model.Email}");

            var existeDni = await _userManager.Users.AnyAsync(x => x.Dni == model.Dni);
            if (existeDni)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el DNI {model.Dni}");

            // Crear usuario
            var usuario = _mapper.Map<UsuarioEntity>(model);
            usuario.UserName = model.Email;

            var createUser = await _userManager.CreateAsync(usuario, model.Password);
            if (!createUser.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, createUser.Errors, "Error al crear el usuario");

            // Asignar rol 
            const string rolCliente = "Cliente";
            var rolResult = await _userManager.AddToRoleAsync(usuario, rolCliente);
            if (!rolResult.Succeeded)
            {
                await _userManager.DeleteAsync(usuario); // compensacion
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, rolResult.Errors, "Error al asignar el rol");
            }

            // Crear cliente
            try
            {
                var cliente = _mapper.Map<ClienteEntity>(model);
                cliente.UsuarioId = usuario.Id;
                cliente.FechaRegistro = DateTime.UtcNow;

                _dataBaseService.Clientes.Add(cliente);
                await _dataBaseService.SaveAsync();

                return ResponseApiService.Response(
                    StatusCodes.Status201Created,
                    new { UsuarioId = usuario.Id, ClienteId = cliente.Id, usuario.Email, usuario.Nombre, usuario.Apellido },
                    "Cliente creado correctamente");
            }
            catch
            {
                // compensación si falla el insert de cliente
                await _userManager.DeleteAsync(usuario);
                throw;
            }
        }
    }

}

