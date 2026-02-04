using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina
{
    public class GetEjerciciosQuery : IGetEjerciciosQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetEjerciciosQuery(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int diaRutinaId)
        {

            var dia = await _dataBaseService.DiasRutinas
                .Include(d => d.Rutina)
                .FirstOrDefaultAsync(d => d.Id == diaRutinaId);

            if (dia == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Día de rutina no encontrado");

            if (dia.Rutina == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "El día no tiene rutina asociada");


            // Traer ejercicios del día
            var ejercicios = await _dataBaseService.Ejercicios
                .Where(e => e.DiaRutinaId == diaRutinaId)
                .OrderBy(e => e.Id)
                .ToListAsync();


            var data = _mapper.Map<List<GetEjerciciosModel>>(ejercicios);

            return ResponseApiService.Response(StatusCodes.Status200OK, data);
        }
    }
}
