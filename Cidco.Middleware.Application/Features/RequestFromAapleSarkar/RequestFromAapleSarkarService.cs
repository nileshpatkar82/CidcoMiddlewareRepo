using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Contracts.Persistance;
using Cidco.Middleware.Application.Features.RequestFromAapleSarkar.Queries.GetAapleSarkar;

namespace Cidco.Middleware.Application.Features.RequestFromAapleSarkar
{
    public class RequestFromAapleSarkarService : IRequestFromAapleSarkar
    {
        private readonly IRequestFromAapleSarkarRepository _requestFromAapleSarkarRepository;

        public RequestFromAapleSarkarService(IRequestFromAapleSarkarRepository requestFromAapleSarkarRepository)
        {
            _requestFromAapleSarkarRepository = requestFromAapleSarkarRepository;
        }

        public Task<string> CreateAsync(GetAapleSarkarQueryDto getAapleSarkarQueryDto)
        {
            throw new NotImplementedException();
        }
        //public async Task<string> CreateAsync(GetAapleSarkarQueryDto getAapleSarkarQueryDto)
        //{
        //    Domain.Entities.TblRequestReceivedFromAapleSarkar tblRequest = new Domain.Entities.TblRequestReceivedFromAapleSarkar();
        //    tblRequest.UserId = getAapleSarkarQueryDto.UserId;
        //    tblRequest.TrackId =    getAapleSarkarQueryDto.TrackId;
        //    tblRequest.ServiceId =  getAapleSarkarQueryDto. ServiceId;
        //    tblRequest.RequestInfo = getAapleSarkarQueryDto.RequestInfo;
        //    tblRequest.RequestReceivedStatus = getAapleSarkarQueryDto.RequestReceivedStatus;
        //    var response = await _requestFromAapleSarkarRepository.GetInformation(tblRequest);
        //    return null;
        //}
    }
}
