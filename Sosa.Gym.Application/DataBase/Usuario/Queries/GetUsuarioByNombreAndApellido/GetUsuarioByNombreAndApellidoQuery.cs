using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByNombreAndApellido
{
    public class GetUsuarioByNombreAndApellidoQuery : IGetUsuarioByNombreAndApellidoQuery
    {

        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;

        public GetUsuarioByNombreAndApellidoQuery(
            IDataBaseService dataBaseService,
            IMapper mapper)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<GetUsuarioByNombreAndApellidoModel> Execute(string nombre, string apellido)
        {
            var user = await _dataBaseService.Usuarios.FirstOrDefaultAsync(x => x.Nombre == nombre && x.Apellido == apellido);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return _mapper.Map<GetUsuarioByNombreAndApellidoModel>(user);

        }
    }
}
