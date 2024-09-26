using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Features.LogInformation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;


namespace Cidco.Middleware.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<ILogInformationService, LogInformationService>();
            
            services.AddScoped<ICPFInfoRESTService, CPFInfoRESTService>();

            //services.AddScoped<IRequestSentToSAPRepository,requestse>
            return services;
        }
    }
}
