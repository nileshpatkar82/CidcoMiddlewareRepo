using AutoMapper;
using Cidco.Middleware.Application.Contracts.Infrastructure;
using Cidco.Middleware.Application.Contracts.Persistance;
using MediatR;
using Newtonsoft.Json;
using System.Data;

namespace Cidco.Middleware.Application.Features.RequestFromAapleSarkar.Queries.GetAapleSarkar
{
    public class GetAapleSarkarQueryHandler : IRequestHandler<GetAapleSarkarQuery, List<ErrorLogResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ICPFInfoRESTService _iCPFInfoRESTService;
        private readonly ILogInformationRepository _logInformationRepository;
        public GetAapleSarkarQueryHandler(IMapper mapper, ICPFInfoRESTService cPFInfoRESTService, ILogInformationRepository logInformationRepository)
        {
            _mapper = mapper;
            _iCPFInfoRESTService = cPFInfoRESTService;
            _logInformationRepository = logInformationRepository;
        }
        public async Task<List<ErrorLogResponse>> Handle(GetAapleSarkarQuery request, CancellationToken cancellationToken)
        {
            var dataTable = _iCPFInfoRESTService.fnProcCreateDoc();
            Cidco.Middleware.Domain.Entities.LogInformation logInformation = new Cidco.Middleware.Domain.Entities.LogInformation();
            if(dataTable.Count <= 0)
            {
                logInformation.ErrorInfo = "Data Not Availabel";
            }
            else
            {
                logInformation.Response = JsonConvert.SerializeObject(dataTable);
            }
         //   _logInformationRepository.CreateAsync(logInformation);
            return dataTable;
        }


    }
}
