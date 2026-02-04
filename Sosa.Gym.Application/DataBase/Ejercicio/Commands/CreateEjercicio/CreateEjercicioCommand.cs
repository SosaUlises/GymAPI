using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Ejercicio;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio
{
    public class CreateEjercicioCommand : ICreateEjercicioCommand
    {

        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateEjercicioCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;   
        }

        public async Task<BaseResponseModel> Execute(int diaRutinaId, CreateEjercicioModel model, int userId)
        {
            var diaRutina = await _dataBaseService.DiasRutinas
                .Include(d => d.Rutina)
                .FirstOrDefaultAsync(x => x.Id == diaRutinaId);

            if (diaRutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Día de rutina no encontrado");

            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            if (diaRutina.Rutina == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "El día no tiene rutina asociada");

      /*      if (diaRutina.Rutina.ClienteId != clienteId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar ejercicios a una rutina que no te pertenece");
            }
      */
            var ejercicio = _mapper.Map<EjercicioEntity>(model);
            ejercicio.DiaRutinaId = diaRutinaId;

            await _dataBaseService.Ejercicios.AddAsync(ejercicio);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                "Ejercicio creado correctamente");
        }
    }
}
