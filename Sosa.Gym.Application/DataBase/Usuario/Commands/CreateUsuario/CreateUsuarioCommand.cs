using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Domain.Entidades.Usuario;
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

        public async Task<UsuarioEntity> Execute(CreateUsuarioModel model)
        {
            var entity = _mapper.Map<UsuarioEntity>(model);
            entity.UserName = model.Email;

            var result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddToRoleAsync(entity, model.Rol);

            return entity;
        }

    }
}
