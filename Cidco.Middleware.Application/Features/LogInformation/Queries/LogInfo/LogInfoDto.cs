namespace Cidco.Middleware.Application.Features.LogInformation.Queries.LogInfo
{
    public class LogInfoDto
    {
        public int LogId { get; set; }

        public string Source { get; set; }

        public string VenderName { get; set; }

        public dynamic Request { get; set; }

        public dynamic Response { get; set; }

        public dynamic ErrorInfo { get; set; }

        public string UserEmail { get; set; }

        public DateTime? LogDateTime { get; set; }
    }
}
