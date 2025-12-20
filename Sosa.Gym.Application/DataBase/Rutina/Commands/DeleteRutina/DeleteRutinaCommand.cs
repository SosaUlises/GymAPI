using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina
{
    public class DeleteRutinaCommand : IDeleteRutinaCommand
    {
        private readonly IDataBaseService _dataBaseService;

        public DeleteRutinaCommand(
            IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, int userId)
        {
            var rutina = await _dataBaseService.Rutinas
                .FirstOrDefaultAsync(x => x.Id == rutinaId);

            if (rutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            if (rutina.ClienteId != clienteId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes eliminar rutinas que no te pertenecen");
            }

            _dataBaseService.Rutinas.Remove(rutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Rutina eliminada correctamente");
        }
    }
}
