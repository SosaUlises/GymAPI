using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Models;
using System.Security.Claims;

namespace Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes
{
    public class GetCuotasByEstadoQuery : IGetCuotasByEstadoQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _http;

        public GetCuotasByEstadoQuery(
            IDataBaseService dataBaseService,
            IMapper mapper,
            IHttpContextAccessor http)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
            _http = http;
        }

        public async Task<BaseResponseModel> Execute(string estado)
        {
            var estadoLower = estado.ToLower();
            var user = _http.HttpContext.User;

            var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var rol = user.FindFirst(ClaimTypes.Role).Value;

            IQueryable<CuotaEntity> query = _dataBaseService.Cuotas
                .Where(x => x.Estado.ToLower() == estadoLower);

            // Si es cliente: solo sus cuotas
            if (rol == "Cliente")
            {
                query = query.Where(x => x.Cliente.UsuarioId == userId);
            }

            var cuotas = await query
                .OrderByDescending(x => x.Anio)
                .ThenByDescending(x => x.Mes)
                .ToListAsync();

            if (!cuotas.Any())
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "No hay cuotas con ese estado");
            }

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetCuotasByEstadoModel>>(cuotas));
        }
    }
}
