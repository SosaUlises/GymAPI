using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Rutina;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina
{
    public class CreateDiaRutinaCommand : ICreateDiaRutinaCommand
    {

        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateDiaRutinaCommand(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseRespondeModel> Execute(CreateDiaRutinaModel model, int userId)
        {
            var rutina = await _dataBaseService.Rutinas.FindAsync(model.RutinaId);

            if (rutina == null)
            {
                return ResponseApiService.Response(
                        StatusCodes.Status404NotFound,
                        "La rutina no fue encontrada");
            }

            if (rutina.ClienteId != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar días a una rutina que no te pertenece");
            }

            var diaRutina = _mapper.Map<DiasRutinaEntity>(model);
            await _dataBaseService.DiasRutinas.AddAsync(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
               StatusCodes.Status200OK,
               "Dia de la rutina agregado correctamente");
        }
    }
}
