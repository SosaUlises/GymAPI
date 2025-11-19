using Microsoft.AspNetCore.Http;
using Sosa.Gym.Application.Features;
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

        public async Task<BaseRespondeModel> Execute(PagarCuotaModel model)
        {
            var cuota = await _dataBaseService.Cuotas.FindAsync(model.CuotaId);

            if (cuota == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    "La cuota no existe");

            if (cuota.Estado == "Pagado")
                return ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    "La cuota ya fue pagada");

            cuota.Estado = "Pagado";
            cuota.FechaPago = DateTime.UtcNow;
            cuota.MetodoPago = model.MetodoPago;

            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK,
                cuota, "Cuota pagada correctamente");
        }
    }
}
