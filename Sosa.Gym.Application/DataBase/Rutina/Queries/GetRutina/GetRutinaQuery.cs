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
    public class GetRutinaQuery : IGetRutinaQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetRutinaQuery(
            IMapper mapper,
            IDataBaseService dataBaseService
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseResponseModel> Execute(int userId)
        {
            var clienteId = await _dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            var rutinas = await _dataBaseService.Rutinas
                .Where(r => r.ClienteId == clienteId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();

            var data = _mapper.Map<List<GetRutinaModel>>(rutinas);

            return ResponseApiService.Response(StatusCodes.Status200OK, data);
        }

    }
}
