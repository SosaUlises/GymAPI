using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.Configuration;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.AsignarRutina;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.DesasignarRutina;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignada;
using Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignadaDetalle;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteAdmin;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll;
using Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotaByCliente;
using Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.DeleteDiaRutina;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.DeleteEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina;
using Sosa.Gym.Application.DataBase.Login;
using Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso;
using Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByDni;
using Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById;
using Sosa.Gym.Application.Validators.Cliente;
using Sosa.Gym.Application.Validators.Cuota;
using Sosa.Gym.Application.Validators.DiaRutina;
using Sosa.Gym.Application.Validators.Ejercicio;
using Sosa.Gym.Application.Validators.Login;
using Sosa.Gym.Application.Validators.Progreso;
using Sosa.Gym.Application.Validators.Rutina;

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
            services.AddTransient<IGetClienteQuery, GetClienteQuery>();
            services.AddTransient<IGetClienteByDniQuery, GetClienteByDniQuery>();
            services.AddTransient<IGetClienteByIdAdminQuery, GetClienteByIdAdminQuery>();

            // Rutinas
            services.AddTransient<ICreateRutinaCommand, CreateRutinaCommand>();
            services.AddTransient<IUpdateRutinaCommand, UpdateRutinaCommand>();
            services.AddTransient<IDeleteRutinaCommand, DeleteRutinaCommand>();


            // Dia Rutina
            services.AddTransient<ICreateDiaRutinaCommand, CreateDiaRutinaCommand>();
            services.AddTransient<IDeleteDiaRutinaCommand, DeleteDiaRutinaCommand>();

            // Ejercicio
            services.AddTransient<ICreateEjercicioCommand, CreateEjercicioCommand>();
            services.AddTransient<IUpdateEjercicioCommand, UpdateEjercicioCommand>();
            services.AddTransient<IDeleteEjercicioCommand, DeleteEjercicioCommand>();
            services.AddTransient<IGetEjerciciosQuery, GetEjerciciosQuery>();

            // Progreso
            services.AddTransient<ICreateProgresoCommand, CreateProgresoCommand>();
            services.AddTransient<IUpdateProgresoCommand, UpdateProgresoCommand>();
            services.AddTransient<IGetProgresoQuery, GetProgresoQuery>();

            // Cuota
            services.AddTransient<ICreateCuotaCommand, CreateCuotaCommand>();
            services.AddTransient<IGenerarCuotasCommand, GenerarCuotasCommand>();
            services.AddTransient<IPagarCuotaCommand, PagarCuotaCommand>();
            services.AddTransient<IGetCuotaByClienteQuery, GetCuotaByClienteQuery>();
            services.AddTransient<IGetCuotasByEstadoQuery, GetCuotasByEstadoQuery>();

            // Asignacion de rutinas
            services.AddTransient<IAsignarRutinaCommand, AsignarRutinaCommand>();
            services.AddTransient<IDesasignarRutinaCommand, DesasignarRutinaCommand>();
            services.AddTransient<IGetRutinasAsignadasQuery, GetRutinasAsignadasQuery>();
            services.AddTransient<IGetRutinaAsignadaDetalleQuery, GetRutinaAsignadaDetalleQuery>();

            // Login
            services.AddTransient<ILoginCommand, LoginCommand>();

            // Validators
            services.AddScoped<IValidator<CreateClienteModel>, CreateClienteValidator>();
            services.AddScoped<IValidator<UpdateClienteModel>, UpdateClienteValidator>();

            services.AddScoped<IValidator<CreateRutinaModel>, CreateRutinaValidator>();
            services.AddScoped<IValidator<UpdateRutinaModel>, UpdateRutinaValidator>();

            services.AddScoped<IValidator<CreateDiaRutinaModel>, CreateDiaRutinaValidator>();

            services.AddScoped<IValidator<CreateEjercicioModel>, CreateEjercicioValidator>();
            services.AddScoped<IValidator<UpdateEjercicioModel>, UpdateEjercicioValidator>();

            services.AddScoped<IValidator<CreateProgresoModel>, CreateProgresoValidator>();
            services.AddScoped<IValidator<UpdateProgresoModel>, UpdateProgresoValidator>();

            services.AddScoped<IValidator<LoginModel>, LoginValidator>();

            services.AddScoped<IValidator<CreateCuotaModel>, CreateCuotaValidator>();
            services.AddScoped<IValidator<GenerarCuotasModel>, GenerarCuotaValidator>();
            services.AddScoped<IValidator<PagarCuotaModel>, PagarCuotaValidator>();


            return services;
        }
    }
}
