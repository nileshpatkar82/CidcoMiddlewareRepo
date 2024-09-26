using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Features.LogInformation.Queries.LogInfo;
using Cidco.Middleware.Application.Features.RequestFromAapleSarkar.Queries.GetAapleSarkar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cidco.Middleware.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestFromAapleSarkarController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogInformationService _logInfoService;
        private LogInfoDto logInfoDto;
        public RequestFromAapleSarkarController(IMediator mediator, ILogInformationService logInfoService)
        {
            _mediator = mediator;
            _logInfoService = logInfoService;
            logInfoDto = new LogInfoDto();
        }

        [HttpPost]
        public async Task<ActionResult> getAapleSarkarInformation(GetAapleSarkarQuery getAapleSarkarQuery)
        {
            try
            {
                logInfoDto.Source = "RequestFromAapleSarkar/GetInformation";
                logInfoDto.Request = getAapleSarkarQuery;
                logInfoDto.UserEmail = HttpContext.Session.GetString("UserEmail");
                var response = await _mediator.Send(getAapleSarkarQuery);
                logInfoDto.Response = response;
                return Ok(response);
            }
            catch (Exception ex)
            {
                logInfoDto.ErrorInfo = ex.Message + ": Stack trace:" + ex.StackTrace;
                return BadRequest("Something went wrong, please try again later!");
            }
            finally
            {
                logInfoDto.VenderName = string.Empty;
                await _logInfoService.CreateAsync(logInfoDto);
            }
        }
    }
}
