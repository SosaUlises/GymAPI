using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Entrenador.Queries.GetAllEntrenadores
{
    public class GetAllEntrenadoresQuery : IGetAllEntrenadoresQuery
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public GetAllEntrenadoresQuery(UserManager<UsuarioEntity> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int pageNumber, int pageSize)
        {
            var entrenadores = await _userManager.GetUsersInRoleAsync("Entrenador");

            var data = entrenadores
                .OrderBy(x => x.Apellido)
                .ThenBy(x => x.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => _mapper.Map<GetEntrenadorModel>(x))
                .ToList();

            return ResponseApiService.Response(StatusCodes.Status200OK, data);
        }
    }
}
