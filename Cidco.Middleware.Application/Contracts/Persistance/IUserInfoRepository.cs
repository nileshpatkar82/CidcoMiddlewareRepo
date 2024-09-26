using Cidco.Middleware.Domain.Entities;

namespace Cidco.Middleware.Application.Contracts.Persistance
{
    public interface IUserInfoRepository
    {
        Task<Apiuser> GetUser(string email, string password);
    }
}
