using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BaseResponseModel> Execute(int cuotaId, PagarCuotaModel model, int userId)
        {
            var cuota = await _dataBaseService.Cuotas
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == cuotaId);

            if (cuota == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "La cuota no existe");

            if (cuota.Cliente == null)
                return ResponseApiService.Response(StatusCodes.Status500InternalServerError, "La cuota no tiene cliente asociado");

            if (cuota.Cliente.UsuarioId != userId)
                return ResponseApiService.Response(StatusCodes.Status403Forbidden, "No puedes pagar la cuota de otro cliente");

            if (cuota.Estado == "Pagado")
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "La cuota ya fue pagada");

            cuota.Estado = "Pagado";
            cuota.FechaPago = DateTime.UtcNow;
            cuota.MetodoPago = model.MetodoPago;

            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Cuota pagada correctamente");
        }
    }
}
