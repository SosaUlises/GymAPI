using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.AsignarRutina.Commands.DesasignarRutina
{
    public class DesasignarRutinaCommand : IDesasignarRutinaCommand
    {
        private readonly IDataBaseService _db;

        public DesasignarRutinaCommand(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, int clienteId)
        {
            if (rutinaId <= 0 || clienteId <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Ids inválidos");

            var asignacion = await _db.RutinasAsignadas
                .FirstOrDefaultAsync(x => x.RutinaId == rutinaId && x.ClienteId == clienteId);

            if (asignacion == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Asignación no encontrada");

            _db.RutinasAsignadas.Remove(asignacion);
            await _db.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Rutina desasignada correctamente");
        }
    }
}
