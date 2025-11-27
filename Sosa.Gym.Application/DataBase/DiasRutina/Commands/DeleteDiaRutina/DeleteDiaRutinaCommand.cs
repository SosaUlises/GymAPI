using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina
{
    public class DeleteDiaRutinaCommand : IDeleteDiaRutinaCommand
    {
        private readonly IDataBaseService _dataBaseService;

        public DeleteDiaRutinaCommand(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseRespondeModel> Execute(int idDiaRutina, int userId)
        {
            var diaRutina = await _dataBaseService.DiasRutinas
                                                    .Include(d => d.Rutina)
                                                    .FirstOrDefaultAsync(x => x.Id == idDiaRutina);

            if (diaRutina == null) 
            {
                return ResponseApiService.Response(
                       StatusCodes.Status404NotFound,
                       "El Dia de la rutina no fue encontrada");
            }

            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (diaRutina.Rutina.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes eliminar días a una rutina que no te pertenece");
            }

            _dataBaseService.DiasRutinas.Remove(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Rutina borrada correctamente");
        }
    }
}
