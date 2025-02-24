//using AutoMapper;
//using DomainSharedKernel.Interfaces;
//using MassTransit;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using SagaOrchestrationStateMachines.Common.DTO;
//using SagaOrchestrationStateMachines.Common.Interfaces;
//using SagaOrchestrationStateMachines.Common.Specifications;

//namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Features.Queries;

//public sealed class GetUserCreatedSagOrchestratorInstanceQueryHandler
//    : IRequestHandler<GetUserCreatedSagOrchestratorInstanceQuery, GetUserCreatedSagOrchestratorInstanceResponse>
//{
//    private readonly IMySagaRepository<UserCreatedSagaStateInstance> _userCreatedSagaStateInstanceRepository;
//    private readonly ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> _logger;
//    private readonly IMapper _mapper;

//    private readonly DbSet<UserCreatedSagaStateInstance> _usersDbContext;
//    private readonly IMySagaRepository<UserCreatedSagaStateInstance> _mySagaRepository;

//    public GetUserCreatedSagOrchestratorInstanceQueryHandler(
//        IMySagaRepository<UserCreatedSagaStateInstance> userCreatedSagaStateInstanceRepository, 
//        ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> logger, 
//        IMapper mapper, DbSet<UserCreatedSagaStateInstance> usersDbContext, 
//        IMySagaRepository<UserCreatedSagaStateInstance> mySagaRepository)
//    {
//        _userCreatedSagaStateInstanceRepository = userCreatedSagaStateInstanceRepository;
//        _logger = logger;
//        _mapper = mapper;
//        _usersDbContext = usersDbContext;
//        _mySagaRepository = mySagaRepository;
//    }

//    public async Task<GetUserCreatedSagOrchestratorInstanceResponse> Handle(GetUserCreatedSagOrchestratorInstanceQuery request, CancellationToken cancellationToken)
//    {
//        var getUserCreatedSagOrchestratorInstanceResponse = new GetUserCreatedSagOrchestratorInstanceResponse
//        {
//            UserCreatedSagOrchestratorInstanceResponseDto = new()
//        };

//        var spec = new GetUserCreatedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

//        var userCreatedStateInstance = await _userCreatedSagaStateInstanceRepository.FindAsync(spec);

//        var uCSIn = await _usersDbContext.FirstOrDefaultAsync(h => h.CorrelationId == request.CorrelationId, cancellationToken);

//        var uCSMSR = await _mySagaRepository.

//        if (userCreatedStateInstance == null)
//        {
//            getUserCreatedSagOrchestratorInstanceResponse.Success = false;
//            getUserCreatedSagOrchestratorInstanceResponse.Message = $"You made a Bad Request.";
//            getUserCreatedSagOrchestratorInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = null;

//            return getUserCreatedSagOrchestratorInstanceResponse;
//        }

//        getUserCreatedSagOrchestratorInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = _mapper.Map<UserCreatedSagOrchestratorInstanceResponseDto>(userCreatedStateInstance);
//        getUserCreatedSagOrchestratorInstanceResponse.Success = true;
//        getUserCreatedSagOrchestratorInstanceResponse.Message = $"This resource matched your search";

//        return getUserCreatedSagOrchestratorInstanceResponse;
//    }
//}
