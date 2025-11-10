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
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;

        public CreateUsuarioCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UsuarioEntity> Execute(CreateUsuarioModel model)
        {
            var existe = _dataBaseService.Usuarios.FirstOrDefault(x=> x.Email == model.Email);


            if (existe != null) throw new Exception($"Ya existe un usuario registrado con el email {model.Email}");

            var entity = _mapper.Map<UsuarioEntity>(model);
            entity.UserName = model.Email;

            var result = await _userManager.CreateAsync(entity, model.Password);

            if (!result.Succeeded) throw new Exception(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddToRoleAsync(entity, model.Rol);

            return entity;

        }

    }
}
