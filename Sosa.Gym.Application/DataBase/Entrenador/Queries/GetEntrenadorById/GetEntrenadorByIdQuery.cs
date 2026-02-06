using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Application.DataBase.Entrenador.Queries.GetAllEntrenadores;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Queries.GetEntrenadorById
{
    public class GetEntrenadorByIdQuery : IGetEntrenadorByIdQuery
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public GetEntrenadorByIdQuery(UserManager<UsuarioEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Entrenador"))
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            return ResponseApiService.Response(StatusCodes.Status200OK, _mapper.Map<GetEntrenadorModel>(user));
        }
    }
}
