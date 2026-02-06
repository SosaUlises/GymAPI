using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Entrenador.Queries.GetAllEntrenadores;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Queries.GetEntrenadorByDni
{
    public class GetEntrenadorByDniQuery : IGetEntrenadorByDniQuery
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public GetEntrenadorByDniQuery(UserManager<UsuarioEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(long dni)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Dni == dni);
            if (user == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Entrenador"))
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Entrenador no encontrado");

            return ResponseApiService.Response(StatusCodes.Status200OK, _mapper.Map<GetEntrenadorModel>(user));
        }
    }
}
