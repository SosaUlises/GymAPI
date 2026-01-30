using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll
{
    public class GenerarCuotasCommand : IGenerarCuotasCommand
    {
        private readonly IDataBaseService _db;

        public GenerarCuotasCommand(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(GenerarCuotasModel model)
        {

            var clienteIds = await _db.Clientes
                .Select(c => c.Id)
                .ToListAsync();

            if (clienteIds.Count == 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "No hay clientes registrados");

            // Cuotas existentes para ese mes/año (solo ids)
            var existentes = await _db.Cuotas
                .Where(c => c.Anio == model.Anio && c.Mes == model.Mes)
                .Select(c => c.ClienteId)
                .ToListAsync();

            var existentesSet = existentes.ToHashSet();

            // Crear las faltantes
            var nuevas = clienteIds
                .Where(id => !existentesSet.Contains(id))
                .Select(id => new CuotaEntity
                {
                    ClienteId = id,
                    Monto = model.Monto,
                    Anio = model.Anio,
                    Mes = model.Mes,
                    Estado = EstadoCuota.Pendiente,
                    FechaCreacion = DateTime.UtcNow,
                    FechaPago = null,
                    MetodoPago = null
                })
                .ToList();

            if (nuevas.Count == 0)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status200OK,
                    "Todas las cuotas de ese mes ya existen");
            }

            await _db.Cuotas.AddRangeAsync(nuevas);
            await _db.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                $"Cuotas generadas correctamente. Creadas: {nuevas.Count}. Ya existían: {clienteIds.Count - nuevas.Count}");
        }
    }
}
