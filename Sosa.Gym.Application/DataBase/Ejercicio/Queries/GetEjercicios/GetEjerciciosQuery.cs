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

        public async Task<BaseResponseModel> Execute(int diaRutinaId, int userId)
        {
            // ClienteId del user
            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            // Verificar que el día exista y pertenezca al cliente
            var dia = await _dataBaseService.DiasRutinas
                .Include(d => d.Rutina)
                .FirstOrDefaultAsync(d => d.Id == diaRutinaId);

            if (dia == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Día de rutina no encontrado");

            if (dia.Rutina == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "El día no tiene rutina asociada");

            if (dia.Rutina.ClienteId != clienteId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes ver ejercicios de una rutina que no te pertenece");
            }

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
