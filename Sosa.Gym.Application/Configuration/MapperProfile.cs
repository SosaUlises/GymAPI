using AutoMapper;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
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
            CreateMap<UsuarioEntity, CreateUsuarioModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()) 
            .ReverseMap()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<UsuarioEntity, GetAllUsuariosModel>().ReverseMap();
            CreateMap<UsuarioEntity, GetUsuarioByIdModel>().ReverseMap();
            CreateMap<UsuarioEntity, GetUsuarioByDniModel>().ReverseMap();

        }
    }
}
