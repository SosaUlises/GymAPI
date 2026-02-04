using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task<BaseResponseModel> Execute(int rutinaId, CreateDiaRutinaModel model)
        {
            var rutinaExiste = await _dataBaseService.Rutinas.AnyAsync(r => r.Id == rutinaId);
            if (!rutinaExiste)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "La rutina no fue encontrada");

            var existeDia = await _dataBaseService.DiasRutinas
                .AnyAsync(d => d.RutinaId == rutinaId && d.NombreDia == model.NombreDia);

            if (existeDia)
                return ResponseApiService.Response(StatusCodes.Status409Conflict, "Ese día ya existe en la rutina");

            var diaRutina = _mapper.Map<DiasRutinaEntity>(model);
            diaRutina.RutinaId = rutinaId;

            await _dataBaseService.DiasRutinas.AddAsync(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status201Created, new { diaRutina.Id }, "Día agregado correctamente");
        }

    }
}
