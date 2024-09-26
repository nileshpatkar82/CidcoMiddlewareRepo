using AutoMapper;
using Cidco.Middleware.Application.Features.UserInfo.Queries.GetUserInfo;
using Cidco.Middleware.Domain.Entities;

namespace Cidco.Middleware.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Apiuser, GetUserInfoQueryDto>();

            CreateMap<GetUserInfoQueryDto, Apiuser>();

        }
    }
}
