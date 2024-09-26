using MediatR;

namespace Cidco.Middleware.Application.Features.UserInfo.Queries.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<GetUserInfoQueryDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
