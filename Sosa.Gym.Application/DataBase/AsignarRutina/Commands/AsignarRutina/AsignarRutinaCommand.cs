using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Rutina;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.AsignarRutina.Commands.AsignarRutina
{
    public class AsignarRutinaCommand : IAsignarRutinaCommand
    {
        private readonly IDataBaseService _db;

        public AsignarRutinaCommand(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, int clienteId)
        {
            if (rutinaId <= 0 || clienteId <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Ids inválidos");

            var rutinaExiste = await _db.Rutinas.AnyAsync(r => r.Id == rutinaId);
            if (!rutinaExiste)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            var clienteExiste = await _db.Clientes.AnyAsync(c => c.Id == clienteId);
            if (!clienteExiste)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            var yaExiste = await _db.RutinasAsignadas
                .AnyAsync(x => x.RutinaId == rutinaId && x.ClienteId == clienteId);

            if (yaExiste)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status409Conflict,
                    "La rutina ya está asignada a este cliente");
            }

            var nueva = new RutinaAsignadaEntity
            {
                RutinaId = rutinaId,
                ClienteId = clienteId,
                FechaAsignacion = DateTime.UtcNow
            };

            await _db.RutinasAsignadas.AddAsync(nueva);
            await _db.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                new { nueva.Id },
                "Rutina asignada correctamente");
        }
    }
}
