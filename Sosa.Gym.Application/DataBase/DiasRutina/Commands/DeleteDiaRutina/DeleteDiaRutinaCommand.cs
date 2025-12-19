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

        public async Task<BaseResponseModel> Execute(int diaRutinaId, int userId)
        {
            var diaRutina = await _dataBaseService.DiasRutinas
                .Include(d => d.Rutina)
                .FirstOrDefaultAsync(x => x.Id == diaRutinaId);

            if (diaRutina == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "El día de la rutina no fue encontrado");
            }

            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);
            if (cliente == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

            if (diaRutina.Rutina == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status500InternalServerError,
                    "El día no tiene rutina asociada");
            }

            if (diaRutina.Rutina.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes eliminar días de una rutina que no te pertenece");
            }

            _dataBaseService.DiasRutinas.Remove(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Día de la rutina eliminado correctamente");
        }
    }
}
