using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Sosa.Gym.API
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sosa Gym",
                    Description = "Administracion de APIs para Gym App"
                });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, fileName));
            });
            return services;
        }
    }
}
