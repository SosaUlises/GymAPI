using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

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

        public async Task<BaseRespondeModel> Execute(UpdateUsuarioModel model)
        {
            var entity = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (entity == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound
                    , "Usuario no encontrado");
            }

            _mapper.Map(model, entity);

            var result = await _userManager.UpdateAsync(entity);

            if (!result.Succeeded)
            {
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    "Error al modificar el usuario");
            }

            return ResponseApiService.Response(StatusCodes.Status200OK,
                 result);
        }
    }
}
