using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public class GetUsuarioByIdQuery : IGetUsuarioByIdQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;
        public GetUsuarioByIdQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;   
        }

        public async Task<GetUsuarioByIdModel> Execute(int userId)
        {
            var user = await _dataBaseService.Usuarios.FirstOrDefaultAsync(x=> x.Id == userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return _mapper.Map<GetUsuarioByIdModel>(user);
        }
    }
}
