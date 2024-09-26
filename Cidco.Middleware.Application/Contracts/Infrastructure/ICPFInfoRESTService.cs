using Cidco.Middleware.Application.Features.RequestFromAapleSarkar;
using System.Data;

namespace Cidco.Middleware.Application.Contracts.Infrastructure
{
    public interface ICPFInfoRESTService
    {
        public List<ErrorLogResponse> fnProcCreateDoc();
    }
}
