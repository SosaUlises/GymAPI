using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteAdmin
{
    public class GetClienteByIdAdminQuery : IGetClienteByIdAdminQuery
    {
        private readonly IDataBaseService _db;
        private readonly IMapper _mapper;

        public GetClienteByIdAdminQuery(IDataBaseService db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> Execute(int clienteId)
        {
            var cliente = await _db.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == clienteId);

            if (cliente == null)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }

            return ResponseApiService.Response(
                StatusCodes.Status200OK,
                _mapper.Map<GetClienteModel>(cliente));
        }
    }
}
