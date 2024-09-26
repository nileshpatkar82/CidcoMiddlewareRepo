using Cidco.Middleware.Application.Features.RequestFromAapleSarkar.Queries.GetAapleSarkar;

namespace Cidco.Middleware.Application.Contracts.Infrastructure
{
    public interface IRequestFromAapleSarkar
    {
        Task<string> CreateAsync(GetAapleSarkarQueryDto getAapleSarkarQueryDto);

    }
}
