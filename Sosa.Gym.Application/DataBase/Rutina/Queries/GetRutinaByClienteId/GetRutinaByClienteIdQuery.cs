using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId
{
    public class GetRutinaByClienteIdQuery : IGetRutinaByClienteIdQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetRutinaByClienteIdQuery(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int clienteId, int userId)
        {
            var rutinas = await _dataBaseService.Rutinas   
                                                     .Where(x => x.ClienteId == clienteId)
                                                     .ToListAsync();

            if (rutinas == null || !rutinas.Any())
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "No se encontraron rutinas para este cliente");

            var clienteIdDeLaRutina = rutinas.First().ClienteId;
            var cliente = await _dataBaseService.Clientes
                               .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (clienteIdDeLaRutina != cliente.Id)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes ver las rutinas que no te pertenecen");
            }


            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<List<GetRutinaByClienteIdModel>>(rutinas));
        }

    }
}
