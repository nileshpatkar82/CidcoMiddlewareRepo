using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Domain;
using Cidco.Middleware.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Cidco.Middleware.Persistense.Repositories
{
    public class LogInformationRepository : BaseRepository<LogInformation>, ILogInformationRepository
    {
        private readonly ILogger _logger;
        public LogInformationRepository(CidcoMiddlewareDBContext dbContext, ILogger<LogInformation> logger) : base(dbContext, logger)
        {
            _logger = logger;
        }
        public async Task<LogInformation> CreateAsync(LogInformation logInfo)
        {
            var data = new Dictionary<string, object>();
            //data.Add("@RequestId", logInfo.RequestId);
            //data.Add("@ResponseId", logInfo.ResponseId);
            //data.Add("@AppId", logInfo.AppId);
            //data.Add("@Source", logInfo.Source);
            //data.Add("@VenderName", logInfo.VenderName);
            //data.Add("@Request", logInfo.Request);
            data.Add("@Response", logInfo.Response);
            //data.Add("@UserEmail", logInfo.UserEmail);
            data.Add("@ErrorInfo", logInfo.ErrorInfo);
            //data.Add("@CreatedBy", logInfo.CreatedBy);
            //data.Add("@CreatedDate", logInfo.CreatedDate);
            //data.Add("@ModifiedBy", logInfo.ModifiedBy);
            //data.Add("@ModifiedDate", logInfo.ModifiedDate);

            var logList = InsertRecordAsync("STP_AddLogInfo", data).Result;
            return null;
        }
    }
}
