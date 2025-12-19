using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Rutina;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId
{
    public class GetDiasRutinaByRutinaIdQuery : IGetDiasRutinaByRutinaIdQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetDiasRutinaByRutinaIdQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId, int userId)
        {
            var diasRutina = await _dataBaseService.DiasRutinas
                                        .Where(x=> x.RutinaId == rutinaId)  
                                        .Include(d=> d.Rutina)
                                        .ToListAsync();

       
            if (diasRutina == null || !diasRutina.Any())
                return ResponseApiService.Response(StatusCodes.Status404NotFound, 
                    "No se encontraron dias para esta rutina");

            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            var clienteIdDeLaRutina = diasRutina.First().Rutina.ClienteId;

            if (clienteIdDeLaRutina != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes ver los días de una rutina que no te pertenece");
            }

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetDiasRutinaByRutinaIdModel>>(diasRutina));
        }
    }
}
