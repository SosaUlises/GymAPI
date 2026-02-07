using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota
{
    public class PagarCuotaCommand : IPagarCuotaCommand
    {
        private readonly IDataBaseService _dataBaseService;

        public PagarCuotaCommand(
            IDataBaseService db
            )
        {
            _dataBaseService = db;
        }

        public async Task<BaseResponseModel> Execute(int cuotaId, PagarCuotaModel model, int userId)
        {
            var cuota = await _dataBaseService.Cuotas
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == cuotaId);

            if (cuota == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "La cuota no existe");

            if (cuota.Cliente == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "La cuota no tiene cliente asociado");


            if (cuota.Estado == EstadoCuota.Pagada)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "La cuota ya fue pagada");

            cuota.Estado = EstadoCuota.Pagada;
            cuota.FechaPago = DateTime.UtcNow;
            cuota.MetodoPago = model.MetodoPago;

            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Pago registrado correctamente");
        }

    }
}
