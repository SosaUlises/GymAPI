using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina
{
    public class DeleteDiaRutinaCommand : IDeleteDiaRutinaCommand
    {
        private readonly IDataBaseService _dataBaseService;

        public DeleteDiaRutinaCommand(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int diaRutinaId)
        {
            var diaRutina = await _dataBaseService.DiasRutinas
            .FirstOrDefaultAsync(x => x.Id == diaRutinaId);

            if (diaRutina == null)
                return ResponseApiService.Response(404, "El día de la rutina no fue encontrado");

            _dataBaseService.DiasRutinas.Remove(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(200, "Día de la rutina eliminado correctamente");

        }
    }
}
