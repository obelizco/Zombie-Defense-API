using Microsoft.Extensions.DependencyInjection;
using ZombieDefense.Infraestructure.Persistence.Contexts;
using ZombieDefense.Infraestructure.Repositories.RepositoryBase;

namespace ZombieDefense.Infraestructure.DependencyInjection
{
    public static class RepositoryInjection
    {
        public static IServiceCollection AgregarRepositorios(this IServiceCollection services)
        {
            services.AddScoped<AuditLogInterceptor>();
            services.AddScoped(typeof(IZombieDefenseRepository<>), typeof(ZombieDefenseRepository<>));
            return services;
        }
    }
}
