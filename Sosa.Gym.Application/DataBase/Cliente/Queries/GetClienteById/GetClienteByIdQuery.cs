using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni
{
    public class GetClienteByIdQuery : IGetClienteByIdQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;

        public GetClienteByIdQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
        }

        public async Task<BaseRespondeModel> Execute(int clienteId)
        {
            var cliente = await _dataBaseService.Clientes
                                            .Include(x=>x.Usuario)
                                            .FirstOrDefaultAsync(x => x.Id == clienteId);
           
            if (cliente == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

            return ResponseApiService.Response(StatusCodes.Status200OK,
                   _mapper.Map<GetClienteByIdModel>(cliente));
        }
    }
}
