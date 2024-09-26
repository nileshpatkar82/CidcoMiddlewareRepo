using AutoMapper;
using Cidco.Middleware.Application.Contracts.Persistance;
using MediatR;

namespace Cidco.Middleware.Application.Features.UserInfo.Queries.GetUserInfo
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery,GetUserInfoQueryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserInfoRepository _userInfoRepository;
        public GetUserInfoQueryHandler(IUserInfoRepository userInfoRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        public async Task<GetUserInfoQueryDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var objUser = await _userInfoRepository.GetUser(request.Email, request.Password);
            var userinfo = _mapper.Map<GetUserInfoQueryDto>(objUser);
            return userinfo;
        }




    }
}
