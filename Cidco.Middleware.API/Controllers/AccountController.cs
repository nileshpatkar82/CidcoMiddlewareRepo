using AutoMapper;
using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Features.UserInfo.Queries.GetUserInfo;
using Cidco.Middleware.Application.Models.Response;
using Cidco.Middleware.Domain;
using Cidco.Middleware.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cidco.Middleware.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly CidcoMiddlewareDBContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IMediator mediator, ILogger<AccountController> logger, ITokenService tokenService, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("GenerateToken")]
        public async Task<ActionResult> GenerateToken([FromBody] GetUserInfoQuery getUserInfoQuery)
        {
            var response = await _mediator.Send(getUserInfoQuery);
            TokenResponse tokenResponse = new TokenResponse();
            if (response != null)
            {
                var userinfo = _mapper.Map<Apiuser>(response);
                string generatedToken = _tokenService.GetToken(userinfo);
                tokenResponse = new TokenResponse
                {
                    email = userinfo.Email,
                    tokenGeneratedDate = DateTime.Now,
                    token = generatedToken
                };
                HttpContext.Session.SetString("UserEmail", getUserInfoQuery.Email);
            }
            return Ok(tokenResponse);
        }

    }
}
