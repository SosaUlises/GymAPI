using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente
{
    public class GetProgresoByClienteQuery : IGetProgresoByClienteQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetProgresoByClienteQuery(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseRespondeModel> Execute(int clienteId)
        {
            var cliente = await _dataBaseService.Clientes.FirstOrDefaultAsync(x => x.Id == clienteId);

            if(cliente == null )
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            var progresos = await _dataBaseService.Progresos
                                                  .Where(x=>x.ClienteId == clienteId)
                                                  .ToListAsync();

            if (progresos == null || !progresos.Any())
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "No hay progresos cargados");

                return ResponseApiService.Response
                (StatusCodes.Status200OK,
                _mapper.Map<List<GetProgresoByClienteModel>>(progresos));
        }
    }
}
