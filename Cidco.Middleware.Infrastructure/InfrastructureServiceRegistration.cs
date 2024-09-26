using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Infrastructure.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cidco.Middleware.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<ITokenService, TokenService>();

            
            return services;
        }
    }
}
