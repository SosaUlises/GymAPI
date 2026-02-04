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

        public async Task<BaseResponseModel> Execute(int rutinaId)
        {
            var rutina = await _dataBaseService.Rutinas
                .FirstOrDefaultAsync(x => x.Id == rutinaId);

            if (rutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            _dataBaseService.Rutinas.Remove(rutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Rutina eliminada correctamente");
        }
    }
}
