using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota
{
    public class CreateCuotaCommand : ICreateCuotaCommand
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public CreateCuotaCommand(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int clienteId, CreateCuotaModel model)
        {
            var cliente = await _dataBaseService.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "El cliente no existe");

            var existeCuota = await _dataBaseService.Cuotas
                .AnyAsync(x => x.ClienteId == clienteId && x.Anio == model.Anio && x.Mes == model.Mes);

            if (existeCuota)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Ya existe una cuota para ese mes");

            var cuota = _mapper.Map<CuotaEntity>(model);
            cuota.ClienteId = clienteId;

            await _dataBaseService.Cuotas.AddAsync(cuota);
            await _dataBaseService.SaveAsync();

            return ResponseApiService.Response(StatusCodes.Status201Created, "Cuota creada correctamente");
        }
    }
}
