using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotaByCliente;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes
{
    public class GetCuotasByEstadoQuery : IGetCuotasByEstadoQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        public GetCuotasByEstadoQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<BaseRespondeModel> Execute(string estado)
        {

            var estadoLower = estado.ToLower();

            var cuotas = await _dataBaseService.Cuotas
                .Where(x => x.Estado.ToLower() == estadoLower)
                .OrderByDescending(x => x.Anio)
                .ThenByDescending(x => x.Mes)
                .ToListAsync();

            if (cuotas == null || !cuotas.Any())
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "No hay cuotas con ese estado");
            }

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetCuotasByEstadoModel>>(cuotas));
        }
    }
}
