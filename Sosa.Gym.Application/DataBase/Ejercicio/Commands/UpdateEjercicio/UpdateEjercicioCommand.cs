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

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio
{
    public class UpdateEjercicioCommand : IUpdateEjercicioCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public UpdateEjercicioCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int ejercicioId, UpdateEjercicioModel model, int userId)
        {
            var ejercicio = await _dataBaseService.Ejercicios
                .Include(e => e.DiasRutina)
                .ThenInclude(dr => dr.Rutina)
                .FirstOrDefaultAsync(x => x.Id == ejercicioId);

            if (ejercicio == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Ejercicio no encontrado");

            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            if (ejercicio.DiasRutina?.Rutina == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "El ejercicio no tiene rutina asociada");

           /* if (ejercicio.DiasRutina.Rutina.ClienteId != clienteId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes modificar ejercicios que no te pertenecen");
            }*/

            ejercicio.Nombre = model.Nombre;
            ejercicio.Series = model.Series;
            ejercicio.Repeticiones = model.Repeticiones;
            ejercicio.PesoUtilizado = model.PesoUtilizado;

            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Ejercicio actualizado correctamente");
        }

    }
}
