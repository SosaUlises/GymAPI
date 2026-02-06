using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.CreateEntrenador
{
    public class CreateEntrenadorCommand : ICreateEntrenadorCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public CreateEntrenadorCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(CreateEntrenadorModel model)
        {
            var email = model.Email.Trim();

            var existeEmail = await _userManager.Users.AnyAsync(x => x.Email == email);
            if (existeEmail)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, $"Ya existe un usuario con el email {email}");

            var existeDni = await _userManager.Users.AnyAsync(x => x.Dni == model.Dni);
            if (existeDni)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, $"Ya existe un usuario con el DNI {model.Dni}");

            var user = _mapper.Map<UsuarioEntity>(model);
            user.Email = email;
            user.UserName = email;
            user.EmailConfirmed = true;

            var createRes = await _userManager.CreateAsync(user, model.Password);
            if (!createRes.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, createRes.Errors, "Error al crear entrenador");

            var addRole = await _userManager.AddToRoleAsync(user, "Entrenador");
            if (!addRole.Succeeded)
            {
                await _userManager.DeleteAsync(user); 
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, addRole.Errors, "Error al asignar rol Entrenador");
            }

            return ResponseApiService.Response(StatusCodes.Status201Created, new { user.Id }, "Entrenador creado correctamente");
        }
    }
}
