using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId
{
    public class GetDiaRutinaQuery : IGetDiaRutinaQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetDiaRutinaQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, int userId)
        {
            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

          /* var rutinaExisteYEsMia = await _dataBaseService.Rutinas
       .AnyAsync(r => r.Id == rutinaId && r.ClienteId == clienteId);

         if (!rutinaExisteYEsMia)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "Rutina no encontrada");
            }*/

            var dias = await _dataBaseService.DiasRutinas
                .Where(d => d.RutinaId == rutinaId)
                .OrderBy(d => d.Id)
                .ToListAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetDiaRutinaModel>>(dias));
        }
    }
}
