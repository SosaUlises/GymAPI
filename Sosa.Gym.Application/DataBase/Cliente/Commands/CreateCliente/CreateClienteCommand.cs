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

        public async Task<BaseRespondeModel> Execute(CreateClienteModel model)
        {
            var existeEmail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            var existeDni = await _userManager.Users.FirstOrDefaultAsync(x => x.Dni == model.Dni);

            if (existeEmail != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el email {model.Email}");

            if (existeDni != null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el DNI {model.Dni}");

            // Usuario
            var usuario = _mapper.Map<UsuarioEntity>(model);
            usuario.UserName = model.Email;

            var result = await _userManager.CreateAsync(usuario, model.Password);

            if (!result.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    result.Errors, "Error al crear el usuario");

            var rolResult = await _userManager.AddToRoleAsync(usuario, model.Rol);
            if (!rolResult.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    rolResult.Errors, "Error al asignar el rol al usuario");

            // Cliente
            var cliente = _mapper.Map<ClienteEntity>(model);
            cliente.UsuarioId = usuario.Id;
            cliente.FechaRegistro = DateTime.UtcNow;

            _dataBaseService.Clientes.Add(cliente);
            await _dataBaseService.SaveAsync();


            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                new
                {
                    UsuarioId = usuario.Id,
                    cliente.Id,
                    usuario.Email,
                    usuario.Nombre,
                    usuario.Apellido
                },
                "Cliente creado correctamente"
            );
        }
    }

}

