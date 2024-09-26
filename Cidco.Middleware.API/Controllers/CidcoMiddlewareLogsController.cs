using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Features.LogInformation.Queries.LogGeneration;
using Cidco.Middleware.Application.Features.LogInformation.Queries.LogInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cidco.Middleware.API.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    [Route("cidco")]
    public class CidcoMiddlewareLogsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogInformationService _logInfoService;
        private LogInfoDto logInfoDto;

        public CidcoMiddlewareLogsController(IMediator mediator, ILogInformationService logInfoService)
        {
            _mediator = mediator;
            _logInfoService = logInfoService;
            logInfoDto = new LogInfoDto();
        }

        [HttpPost]
        [Route("LogInformation")]
        public async Task<ActionResult> GetLogInformation(LogGenerationQuery logGenerationQuery)
        {
            try
            {
                logInfoDto.Source = "CidcoMiddlewareCRUD/GetLogInformation";
                logInfoDto.Request = logGenerationQuery;
                logInfoDto.UserEmail = HttpContext.Session.GetString("UserEmail");
                var response = await _mediator.Send(logGenerationQuery);
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
