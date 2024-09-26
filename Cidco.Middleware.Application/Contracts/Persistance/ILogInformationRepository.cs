using Cidco.Middleware.Domain.Entities;

namespace Cidco.Middleware.Application.Contracts.Persistance
{
    public interface ILogInformationRepository : IAsyncRepository<LogInformation>
    {
        Task<LogInformation> CreateAsync(LogInformation loginfo);
    }
}
