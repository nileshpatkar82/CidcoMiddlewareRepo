using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Domain;
using Cidco.Middleware.Persistense.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cidco.Middleware.Persistense
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbProvider = configuration.GetSection("dbProvider").Value;

            switch (dbProvider)
            {
                case "MSSQL":
                    services.AddDbContext<CidcoMiddlewareDBContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("EODBConnectString")));
                    break;
                default:
                    break;
            }

            services.AddScoped<IUserInfoRepository, UserInfoRepository>();
            services.AddScoped<ILogInformationRepository, LogInformationRepository>();
            return services;
        }
    }
}
