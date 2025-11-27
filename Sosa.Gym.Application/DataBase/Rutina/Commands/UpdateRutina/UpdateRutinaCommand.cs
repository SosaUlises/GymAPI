using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Rutina;
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

        public async Task<BaseRespondeModel> Execute(UpdateRutinaModel model, int userId)
        {
            var rutina = await _dataBaseService.Rutinas.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (rutina == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            if (rutina.ClienteId != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes modificar rutinas a una cuenta que no te pertenece");
            }

            _mapper.Map(model, rutina);
            _dataBaseService.Rutinas.Update(rutina);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK,
                "Rutina actualizada correctamente");
        }
    }
}
