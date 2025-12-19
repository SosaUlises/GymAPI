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

        public async Task<BaseResponseModel> Execute(UpdateEjercicioModel model, int userId)
        {
            var ejercicio = await _dataBaseService.Ejercicios
                             .Include(e => e.DiasRutina)
                             .ThenInclude(dr => dr.Rutina)
                             .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (ejercicio == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Ejercicio no encontrado");

            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (ejercicio.DiasRutina.Rutina.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes modificar ejercicios que no te pertenecen");
            }

            _mapper.Map(model, ejercicio);
            _dataBaseService.Ejercicios.Update(ejercicio);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Ejercicio actualizado");

        }


    }
}
