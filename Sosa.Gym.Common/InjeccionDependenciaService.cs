using Microsoft.Extensions.DependencyInjection;

namespace Sosa.Gym.Common
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            return services;
        }
    }
}
