using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.Configuration;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Commands.DeleteUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using FluentValidation;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
using Sosa.Gym.Application.Validators.Usuario;

namespace Sosa.Gym.Application
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            // Usuarios
            services.AddTransient<IUpdateUsuarioCommand, UpdateUsuarioCommand>();
            services.AddTransient<IDeleteUsuarioCommand, DeleteUsuarioCommand>();
            services.AddTransient<IGetUsuarioByIdQuery, GetUsuarioByIdQuery>();
            services.AddTransient<IGetAllUsuariosQuery, GetAllUsuariosQuery>();
            services.AddTransient<ICreateUsuarioCommand, CreateUsuarioCommand>();


            // Validators
            services.AddScoped<IValidator<CreateUsuarioModel>, CreateUsuarioValidator>();
            services.AddScoped<IValidator<UpdateUsuarioModel>, UpdateUsuarioValidator>();

            return services;
        }
    }
}
