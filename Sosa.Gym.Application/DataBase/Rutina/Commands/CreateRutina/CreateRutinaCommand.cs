using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Rutina;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina
{
    public class CreateRutinaCommand : ICreateRutinaCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateRutinaCommand(
            IMapper mapper,
            IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;   
        }

        public async Task<BaseResponseModel> Execute(CreateRutinaModel model)
        {

            var rutina = _mapper.Map<RutinaEntity>(model);
            rutina.FechaCreacion = DateTime.UtcNow;

            await _dataBaseService.Rutinas.AddAsync(rutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status201Created, "Rutina creada correctamente");
        }
    }
}
