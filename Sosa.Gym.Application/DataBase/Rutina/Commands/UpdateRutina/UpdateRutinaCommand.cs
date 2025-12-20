using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina
{
    public class UpdateRutinaCommand : IUpdateRutinaCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public UpdateRutinaCommand(
            IMapper mapper,
            IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, UpdateRutinaModel model, int userId)
        {
            var rutina = await _dataBaseService.Rutinas.FirstOrDefaultAsync(x => x.Id == rutinaId);
            if (rutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            if (rutina.ClienteId != clienteId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes modificar rutinas que no te pertenecen");
            }

            rutina.Nombre = model.Nombre;
            rutina.Descripcion = model.Descripcion;

            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, "Rutina actualizada correctamente");
        }
    }
}
