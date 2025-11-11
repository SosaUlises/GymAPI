using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.Configuration;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using FluentValidation;
using Sosa.Gym.Application.Validators.Usuario;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.Validators.Cliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;

namespace Sosa.Gym.Application
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            // Usuarios
            services.AddTransient<IUpdateUsuarioCommand, UpdateUsuarioCommand>();
            services.AddTransient<IGetUsuarioByIdQuery, GetUsuarioByIdQuery>();
            services.AddTransient<IGetAllUsuariosQuery, GetAllUsuariosQuery>();
            services.AddTransient<IGetUsuarioByDniQuery, GetUsuarioByDniQuery>();

            // Clientes
            services.AddTransient<ICreateClienteCommand, CreateClienteCommand>();
            services.AddTransient<IDeleteClienteCommand, DeleteClienteCommand>();

            // Validators
            services.AddScoped<IValidator<UpdateUsuarioModel>, UpdateUsuarioValidator>();
            services.AddScoped<IValidator<CreateClienteModel>, CreateClienteValidator>();

            return services;
        }
    }
}
