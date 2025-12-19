using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.DeleteEjercicio
{
    public class DeleteEjercicioCommand : IDeleteEjercicioCommand
    {
        private readonly IDataBaseService _dataBaseService;

        public DeleteEjercicioCommand(
            IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int ejercicioId, int userId)
        {
            var ejercicio = await _dataBaseService.Ejercicios
                              .Include(e => e.DiasRutina)
                              .ThenInclude(dr => dr.Rutina)
                              .FirstOrDefaultAsync(x => x.Id == ejercicioId);

            if (ejercicio == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Ejercicio no encontrado");

            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (ejercicio.DiasRutina.Rutina.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes eliminar ejercicios que no te pertenecen");
            }

            _dataBaseService.Ejercicios.Remove(ejercicio);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Ejercicio borrado correctamente");
        }
    }
}
