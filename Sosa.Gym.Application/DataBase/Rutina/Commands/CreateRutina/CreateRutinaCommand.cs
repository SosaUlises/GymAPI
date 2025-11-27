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

        public async Task<BaseRespondeModel> Execute(CreateRutinaModel model, int userId)
        {
            var rutina = _mapper.Map<RutinaEntity>(model);
            rutina.FechaCreacion = DateTime.UtcNow;

            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(x => x.Id == model.ClienteId);
            if (cliente == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            var clienteLog = await _dataBaseService.Clientes
                                   .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (rutina.ClienteId != clienteLog.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar rutinas a una cuenta que no te pertenece");
            }

            await _dataBaseService.Rutinas.AddAsync(rutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Rutina creada correctamente");
        }
    }
}
