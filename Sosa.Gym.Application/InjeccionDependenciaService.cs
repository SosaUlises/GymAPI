using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.Configuration;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using FluentValidation;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.Validators.Cliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.Validators.Rutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;

namespace Sosa.Gym.Application
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            // Usuarios
            services.AddTransient<IGetUsuarioByIdQuery, GetUsuarioByIdQuery>();
            services.AddTransient<IGetAllUsuariosQuery, GetAllUsuariosQuery>();
            services.AddTransient<IGetUsuarioByDniQuery, GetUsuarioByDniQuery>();

            // Clientes
            services.AddTransient<ICreateClienteCommand, CreateClienteCommand>();
            services.AddTransient<IDeleteClienteCommand, DeleteClienteCommand>();
            services.AddTransient<IUpdateClienteCommand, UpdateClienteCommand>();
            services.AddTransient<IGetAllClientesQuery, GetAllClientesQuery>();
            services.AddTransient<IGetClienteByIdQuery, GetClienteByIdQuery>();
            services.AddTransient<IGetClienteByDniQuery, GetClienteByDniQuery>();

            // Rutinas
            services.AddTransient<ICreateRutinaCommand, CreateRutinaCommand>();
            services.AddTransient<IUpdateRutinaCommand, UpdateRutinaCommand>();

            // Validators
            services.AddScoped<IValidator<CreateClienteModel>, CreateClienteValidator>();
            services.AddScoped<IValidator<UpdateClienteModel>, UpdateClienteValidator>();

            services.AddScoped<IValidator<CreateRutinaModel>, CreateRutinaValidator>();
            services.AddScoped<IValidator<UpdateRutinaModel>, UpdateRutinaValidator>();

            return services;
        }
    }
}
