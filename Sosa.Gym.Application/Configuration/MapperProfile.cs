using AutoMapper;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Domain.Entidades.Cliente;
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

            // Usuarios
            CreateMap<UsuarioEntity, GetAllUsuariosModel>().ReverseMap();
            CreateMap<UsuarioEntity, GetUsuarioByIdModel>().ReverseMap();
            CreateMap<UsuarioEntity, GetUsuarioByDniModel>().ReverseMap();

            // Clientes
            CreateMap<CreateClienteModel, UsuarioEntity>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreateClienteModel, ClienteEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Usuario, opt => opt.Ignore()) 
                .ForMember(dest => dest.FechaRegistro, opt => opt.Ignore());

            CreateMap<UsuarioEntity, UpdateClienteModel>().ReverseMap();
            CreateMap<ClienteEntity, UpdateClienteModel>().ReverseMap();

            CreateMap<ClienteEntity, GetAllClientesModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Usuario.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Usuario.Apellido))
                .ForMember(dest => dest.Dni, opt => opt.MapFrom(src => src.Usuario.Dni))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email));

            CreateMap<ClienteEntity, GetClienteByIdModel>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Usuario.Apellido))
                .ForMember(dest => dest.Dni, opt => opt.MapFrom(src => src.Usuario.Dni))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email));

            CreateMap<ClienteEntity, GetClienteByDniModel>()
              .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario.Nombre))
              .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Usuario.Apellido))
              .ForMember(dest => dest.Dni, opt => opt.MapFrom(src => src.Usuario.Dni))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email));

        }
    }
}
