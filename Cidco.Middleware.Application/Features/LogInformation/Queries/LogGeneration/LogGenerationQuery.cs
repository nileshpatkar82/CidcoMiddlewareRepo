using Cidco.Middleware.Application.Models.Response;
using MediatR;

namespace Cidco.Middleware.Application.Features.LogInformation.Queries.LogGeneration
{
    public class LogGenerationQuery : IRequest<AapleSarkarResponse>
    {
        public string ApplicationId { get; set; }
    }
}
