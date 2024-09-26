using Cidco.Middleware.Domain.Entities;

namespace Cidco.Middleware.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        string GetToken(Apiuser aPIUsers);
    }
}
