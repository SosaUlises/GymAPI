using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Common.Cuotas;
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
            var clienteIds = await _db.Clientes.Select(c => c.Id).ToListAsync();
            if (clienteIds.Count == 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "No hay clientes registrados");

            var existentes = await _db.Cuotas
                .Where(c => c.Anio == model.Anio && c.Mes == model.Mes)
                .Select(c => c.ClienteId)
                .ToListAsync();

            var existentesSet = existentes.ToHashSet();

            const int DIA_VENCIMIENTO = 10;
            var vencimiento = CuotaDateHelper.CalcularVencimientoUtc(model.Anio, model.Mes, DIA_VENCIMIENTO);

            var nuevas = clienteIds
                .Where(id => !existentesSet.Contains(id))
                .Select(id => new CuotaEntity
                {
                    ClienteId = id,
                    Monto = model.Monto,
                    Anio = model.Anio,
                    Mes = model.Mes,
                    FechaVencimiento = vencimiento,
                    Estado = EstadoCuota.Pendiente,
                    FechaCreacion = DateTime.UtcNow,
                    FechaPago = null,
                    MetodoPago = null
                })
                .ToList();

            if (nuevas.Count == 0)
                return ResponseApiService.Response(StatusCodes.Status200OK, "Todas las cuotas de ese mes ya existen");

            await _db.Cuotas.AddRangeAsync(nuevas);
            await _db.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                $"Cuotas generadas correctamente. Creadas: {nuevas.Count}. Ya existían: {clienteIds.Count - nuevas.Count}");
        }

    }
}
