using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaDetalleAdmin
{
    public class GetRutinaAdminDetalleQuery : IGetRutinaAdminDetalleQuery
    {
        private readonly IDataBaseService _db;
        private readonly IMapper _mapper;

        public GetRutinaAdminDetalleQuery(IDataBaseService db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId)
        {
            if (rutinaId <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "RutinaId inválido");

            var rutina = await _db.Rutinas
                .AsNoTracking()
                .Include(r => r.DiasRutina)
                    .ThenInclude(d => d.Ejercicios)
                .FirstOrDefaultAsync(r => r.Id == rutinaId);

            if (rutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            var data = _mapper.Map<GetRutinaAdminDetalleModel>(rutina);

            data.Dias = data.Dias.OrderBy(d => d.Id).ToList();
            foreach (var d in data.Dias)
                d.Ejercicios = d.Ejercicios.OrderBy(e => e.Id).ToList();

            return ResponseApiService.Response(StatusCodes.Status200OK, data);
        }
    }
}
