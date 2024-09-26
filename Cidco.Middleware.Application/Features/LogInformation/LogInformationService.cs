using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Application.Features.LogInformation.Queries.LogInfo;
using Newtonsoft.Json;

namespace Cidco.Middleware.Application.Features.LogInformation
{
    public class LogInformationService : ILogInformationService
    {
        private readonly ILogInformationRepository _logInformationRepository;
        public LogInformationService(ILogInformationRepository logInformationRepository)
        {
            _logInformationRepository = logInformationRepository;
        }
        public async Task<string> CreateAsync(LogInfoDto request)
        {
            Domain.Entities.LogInformation logInfo = new Domain.Entities.LogInformation();
            logInfo.LogId = request.LogId;
            logInfo.Source = request.Source;
            logInfo.VenderName = request.VenderName;
            logInfo.Request = JsonConvert.SerializeObject(request.Request);
            logInfo.Response = JsonConvert.SerializeObject(request.Response);
            logInfo.UserEmail = request.UserEmail;
            logInfo.ErrorInfo = JsonConvert.SerializeObject(request.ErrorInfo);
            var response = await _logInformationRepository.CreateAsync(logInfo);
            return null;
        }
    }
}
