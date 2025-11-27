using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso
{
    public class CreateProgresoCommand : ICreateProgresoCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateProgresoCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseRespondeModel> Execute(CreateProgresoModel model, int userId)
        {
            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(x => x.Id == model.ClienteId);
            if (cliente == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");


            if (cliente.Id != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes agregar progresos a una cuenta que no te pertenece");
            }

            var progreso = _mapper.Map<ProgresoEntity>(model);
            progreso.FechaRegistro = DateTime.UtcNow;
            await _dataBaseService.Progresos.AddAsync(progreso);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                "Progreso creado correctamente");
        }
    }
}
