using Cidco.Middleware.Application.Features.LogInformation.Queries.LogInfo;

namespace Cidco.Middleware.Application.Contracts.Infrastructure
{
    public interface ILogInformationService
    {
        Task<string> CreateAsync(LogInfoDto loginfo);
    }
}
