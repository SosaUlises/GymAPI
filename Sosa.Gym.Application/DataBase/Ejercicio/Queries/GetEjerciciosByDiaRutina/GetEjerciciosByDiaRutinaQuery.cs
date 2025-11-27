using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina
{
    public class GetEjerciciosByDiaRutinaQuery : IGetEjerciciosByDiaRutinaQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetEjerciciosByDiaRutinaQuery(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseRespondeModel> Execute(int diaRutinaId, int userId)
        {
            var ejercicios = await _dataBaseService.Ejercicios
                                                   .Where(x => x.DiaRutinaId == diaRutinaId)
                                                   .Include(e => e.DiasRutina)
                                                   .ThenInclude(dr => dr.Rutina)
                                                   .ToListAsync();

            if(!ejercicios.Any() || ejercicios == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "No se encontraron ejercicios para este dia");

            var clienteIdDeLaRutina = ejercicios.First().DiasRutina.Rutina.ClienteId;

            if (clienteIdDeLaRutina != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes ver los ejercicios de una rutina que no te pertenece");
            }

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetEjerciciosByDiaRutinaModel>>(ejercicios));
        }
    }
}
