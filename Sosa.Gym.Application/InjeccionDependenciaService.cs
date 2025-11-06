using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.Configuration;
using AutoMapper;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.DeleteUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByNombreAndApellido;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;

namespace Sosa.Gym.Application
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            services.AddTransient<IUpdateUsuarioCommand, UpdateUsuarioCommand>();
            services.AddTransient<IDeleteUsuarioCommand, DeleteUsuarioCommand>();
            services.AddTransient<IGetUsuarioByIdQuery, GetUsuarioByIdQuery>();
            services.AddTransient<IGetUsuarioByNombreAndApellidoQuery, GetUsuarioByNombreAndApellidoQuery>();
            services.AddTransient<IGetAllUsuariosQuery, GetAllUsuariosQuery>();


            return services;
        }
    }
}
