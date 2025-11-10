using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario
{
    public class CreateUsuarioCommand : ICreateUsuarioCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public CreateUsuarioCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseRespondeModel> Execute(CreateUsuarioModel model)
        {
            var existeEmail = _userManager.Users.FirstOrDefault(x=> x.Email == model.Email);
            var existeDni = _userManager.Users.FirstOrDefault(x=> x.Dni == model.Dni);
  

            if (existeEmail != null) 
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el email {model.Email}");

            if (existeDni != null) 
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Ya existe un usuario con el DNI {model.Dni}");

            var entity = _mapper.Map<UsuarioEntity>(model);
            entity.UserName = model.Email;

            var result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Error al crear el usuario");

            var rolResult = await _userManager.AddToRoleAsync(entity, model.Rol);

            if (!rolResult.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    $"Error al asignar rol al usuario");

            return ResponseApiService.Response(StatusCodes.Status200OK,
                new { entity.Id, entity.Email, entity.Nombre, entity.Apellido },
                "Usuario creado correctamente");
        }

    }
}
