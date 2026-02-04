using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Ejercicio;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio
{
    public class CreateEjercicioCommand : ICreateEjercicioCommand
    {

        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateEjercicioCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;   
        }

        public async Task<BaseResponseModel> Execute(int diaRutinaId, CreateEjercicioModel model)
        {
            var diaRutinaExiste = await _dataBaseService.DiasRutinas
            .AnyAsync(x => x.Id == diaRutinaId);

            if (!diaRutinaExiste)
                return ResponseApiService.Response(404, "Día de rutina no encontrado");


            var ejercicio = _mapper.Map<EjercicioEntity>(model);
            ejercicio.DiaRutinaId = diaRutinaId;

            await _dataBaseService.Ejercicios.AddAsync(ejercicio);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                "Ejercicio creado correctamente");
        }
    }
}
