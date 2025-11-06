using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Domain.Entidades.Usuario;

namespace Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario
{
    public class UpdateUsuarioCommand : IUpdateUsuarioCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;
        public UpdateUsuarioCommand(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UpdateUsuarioModel> Execute(UpdateUsuarioModel model)
        {
            var entity = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (entity == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            _mapper.Map(model, entity);

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault()?.Description);
            }

            return model;
        }
    }
}
