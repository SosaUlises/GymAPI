using AutoMapper;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UsuarioEntity, UpdateUsuarioModel>().ReverseMap();
        }
    }
}
