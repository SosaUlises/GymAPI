using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sosa.Gym.Application.DataBase;
using Sosa.Gym.Persistence.DataBase;

namespace Sosa.Gym.External
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddExternal(this IServiceCollection services,
            IConfiguration configuration)
        {

            // Conexion a Postgre
            var connectionString = configuration.GetConnectionString("PostgreConnectionString");
            services.AddDbContext<DataBaseService>(options =>
            options.UseNpgsql(connectionString));

            // Inyecciones de dependencia servicios
            services.AddScoped<IDataBaseService, DataBaseService>();

            return services;
        }
    }
}
