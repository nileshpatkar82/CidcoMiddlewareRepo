using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Domain;
using Cidco.Middleware.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cidco.Middleware.Persistense.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly ILogger _logger;
        private readonly CidcoMiddlewareDBContext _cidcoMiddlewareDB;
        public UserInfoRepository(CidcoMiddlewareDBContext cidcoMiddlewareDBContext)
        {
            _cidcoMiddlewareDB = cidcoMiddlewareDBContext;
        }
        public Task<Apiuser> GetUser(string email, string password)
        {
            return _cidcoMiddlewareDB.Apiusers.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        }
    }
}
