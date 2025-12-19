using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso
{
    public class UpdateProgresoCommand : IUpdateProgresoCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public UpdateProgresoCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(UpdateProgresoModel model, int userId)
        {
            var progreso = await _dataBaseService.Progresos
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (progreso == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Progreso no encontrado");

            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (progreso.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes modificar progresos que no te pertenecen");
            }


            _mapper.Map(model, progreso);
            _dataBaseService.Progresos.Update(progreso);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Progreso actualizado");

        }
    }
}
