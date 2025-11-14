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

        public async Task<BaseRespondeModel> Execute(int ejercicioId)
        {
            var ejercicio = await _dataBaseService.Ejercicios.FirstOrDefaultAsync(x => x.Id == ejercicioId);

            if (ejercicio == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Ejercicio no encontrado");

            _dataBaseService.Ejercicios.Remove(ejercicio);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Ejercicio borrado correctamente");
        }
    }
}
