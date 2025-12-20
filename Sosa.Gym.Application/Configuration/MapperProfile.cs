using AutoMapper;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotaByCliente;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina;
using Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Entidades.Ejercicio;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Entidades.Rutina;
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


            // Rutina
            CreateMap<CreateRutinaModel, RutinaEntity>()
                .ForMember(x => x.ClienteId, opt => opt.Ignore())
                .ForMember(x => x.FechaCreacion, opt => opt.Ignore());

            CreateMap<RutinaEntity, UpdateRutinaModel>().ReverseMap();
            CreateMap<RutinaEntity, GetRutinaModel>().ReverseMap();

            // DiaRutina
            CreateMap<CreateDiaRutinaModel, DiasRutinaEntity>()
                .ForMember(d => d.RutinaId, opt => opt.Ignore());
            CreateMap<DiasRutinaEntity, GetDiaRutinaModel>().ReverseMap();

            // Ejercicio
            CreateMap<CreateEjercicioModel, EjercicioEntity>()
                .ForMember(e => e.DiaRutinaId, opt => opt.Ignore());
            CreateMap<EjercicioEntity, GetEjerciciosModel>().ReverseMap();

            // Progreso 
            CreateMap<CreateProgresoModel, ProgresoEntity>()
                .ForMember(x => x.ClienteId, opt => opt.Ignore())
                .ForMember(x => x.FechaRegistro, opt => opt.Ignore());
            CreateMap<UpdateProgresoModel, ProgresoEntity>()
               .ForMember(x => x.Id, opt => opt.Ignore())
               .ForMember(x => x.ClienteId, opt => opt.Ignore())
               .ForMember(x => x.FechaRegistro, opt => opt.Ignore());

            CreateMap<ProgresoEntity, GetProgresoModel>().ReverseMap();


            // Cuota
            CreateMap<CreateCuotaModel, CuotaEntity>()
                .ForMember(x => x.ClienteId, opt => opt.Ignore());
            CreateMap<CuotaEntity, GetCuotaByClienteModel>().ReverseMap();
            CreateMap<CuotaEntity, GetCuotasByEstadoModel>().ReverseMap();
        }
    }
}
