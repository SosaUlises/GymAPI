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

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni
{
    public class GetClienteByDniQuery : IGetClienteByDniQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        public GetClienteByDniQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;   
        }

        public async Task<BaseResponseModel> Execute(long dni)
        {
            var cliente = await _dataBaseService.Clientes
                                          .Include(x => x.Usuario)
                                          .FirstOrDefaultAsync(x => x.Usuario.Dni == dni);

            if (cliente == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

            return ResponseApiService.Response(StatusCodes.Status200OK,
                    _mapper.Map<GetClienteModel>(cliente));
        }
    }
}
