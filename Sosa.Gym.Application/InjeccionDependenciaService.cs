using Microsoft.Extensions.DependencyInjection;

namespace Sosa.Gym.Application
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
