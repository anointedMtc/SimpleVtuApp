//using AutoMapper;
//using DomainSharedKernel.Interfaces;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;
//using SagaOrchestrationStateMachines.Common.DTO;
//using SagaOrchestrationStateMachines.Common.Specifications;
//using SagaOrchestrationStateMachines.Common.Interfaces;

//namespace SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator.Helpers.Features.Queries;

//public sealed class GetVtuAirtimeOrderedSagaStateInstanceQueryHandler
//    : IRequestHandler<GetVtuAirtimeOrderedSagaStateInstanceQuery, GetVtuAirtimeOrderedSagaStateInstanceResponse>
//{
//    private readonly IMySagaRepository<UserCreatedSagaStateInstance> _userCreatedSagaStateInstanceRepository;
//    private readonly ILogger<GetVtuAirtimeOrderedSagaStateInstanceQueryHandler> _logger;
//    private readonly IMapper _mapper;

//    public GetVtuAirtimeOrderedSagaStateInstanceQueryHandler(
//        IMySagaRepository<UserCreatedSagaStateInstance> userCreatedSagaStateInstanceRepository, 
//        ILogger<GetVtuAirtimeOrderedSagaStateInstanceQueryHandler> logger, 
//        IMapper mapper)
//    {
//        _userCreatedSagaStateInstanceRepository = userCreatedSagaStateInstanceRepository;
//        _logger = logger;
//        _mapper = mapper;
//    }

//    public async Task<GetVtuAirtimeOrderedSagaStateInstanceResponse> Handle(GetVtuAirtimeOrderedSagaStateInstanceQuery request, CancellationToken cancellationToken)
//    {
//        var getVtuAirtimeOrderedSagaStateInstanceResponse = new GetVtuAirtimeOrderedSagaStateInstanceResponse
//        {
//            VtuAirtimeSagaOrchestratorInstanceResponseDto = new()
//        };

//        var spec = new GetUserCreatedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

//        var vtuAirtimeSagaStateInstance = await _userCreatedSagaStateInstanceRepository.FindAsync(spec);

//        if (vtuAirtimeSagaStateInstance == null)
//        {
//            getVtuAirtimeOrderedSagaStateInstanceResponse.Success = false;
//            getVtuAirtimeOrderedSagaStateInstanceResponse.Message = $"You made a Bad Request.";
//            getVtuAirtimeOrderedSagaStateInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = null;

//            return getVtuAirtimeOrderedSagaStateInstanceResponse;
//        }

//        getVtuAirtimeOrderedSagaStateInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = _mapper.Map<VtuAirtimeSagaOrchestratorInstanceResponseDto>(vtuAirtimeSagaStateInstance);
//        getVtuAirtimeOrderedSagaStateInstanceResponse.Success = true;
//        getVtuAirtimeOrderedSagaStateInstanceResponse.Message = $"This resource matched your search";

//        return getVtuAirtimeOrderedSagaStateInstanceResponse;
//    }
//}
