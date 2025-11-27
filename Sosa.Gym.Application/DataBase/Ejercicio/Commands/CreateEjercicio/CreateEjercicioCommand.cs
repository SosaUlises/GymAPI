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

        public async Task<BaseRespondeModel> Execute(CreateEjercicioModel model, int userId)
        {
            var diaRutina = await _dataBaseService.DiasRutinas
                .Include(d=> d.Rutina)
                .FirstOrDefaultAsync(x => x.Id == model.DiaRutinaId);

            if (diaRutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Dia de Rutina no encontrado");

            if (diaRutina.Rutina.ClienteId != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar ejercicios a una rutina que no te pertenece");
            }

            var result = _mapper.Map<EjercicioEntity>(model);

            await _dataBaseService.Ejercicios.AddAsync(result);
            await _dataBaseService.SaveAsync();
            return ResponseApiService.Response(StatusCodes.Status200OK,
                "Ejercicio creado correctamente");

        }
    }
}
