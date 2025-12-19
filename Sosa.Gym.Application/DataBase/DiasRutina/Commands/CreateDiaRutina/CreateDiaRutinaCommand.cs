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

        public async Task<BaseResponseModel> Execute(int rutinaId, CreateDiaRutinaModel model, int userId)
        {
            var rutina = await _dataBaseService.Rutinas.FirstOrDefaultAsync(r => r.Id == rutinaId);
            if (rutina == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "La rutina no fue encontrada");
            }

            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);
            if (cliente == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

            if (rutina.ClienteId != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar días a una rutina que no te pertenece");
            }

            var existeDia = await _dataBaseService.DiasRutinas.AnyAsync(d =>
                d.RutinaId == rutinaId && d.NombreDia == model.NombreDia);

            if (existeDia)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Ese día ya existe en la rutina");
            }

            var diaRutina = _mapper.Map<DiasRutinaEntity>(model);
            diaRutina.RutinaId = rutinaId;

            await _dataBaseService.DiasRutinas.AddAsync(diaRutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status201Created,
                "Día de la rutina agregado correctamente");
        }
    }
}
