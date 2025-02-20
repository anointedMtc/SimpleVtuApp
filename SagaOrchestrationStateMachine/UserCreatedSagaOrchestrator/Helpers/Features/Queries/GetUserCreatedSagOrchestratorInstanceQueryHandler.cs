using AutoMapper;
using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Common.DTO;
using SagaOrchestrationStateMachines.Common.Specifications;

namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Features.Queries;

public sealed class GetUserCreatedSagOrchestratorInstanceQueryHandler
    : IRequestHandler<GetUserCreatedSagOrchestratorInstanceQuery, GetUserCreatedSagOrchestratorInstanceResponse>
{
    private readonly IRepository<UserCreatedSagaStateInstance> _userCreatedSagaStateInstanceRepository;
    private readonly ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetUserCreatedSagOrchestratorInstanceQueryHandler(
        IRepository<UserCreatedSagaStateInstance> userCreatedSagaStateInstanceRepository, 
        ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> logger, 
        IMapper mapper)
    {
        _userCreatedSagaStateInstanceRepository = userCreatedSagaStateInstanceRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetUserCreatedSagOrchestratorInstanceResponse> Handle(GetUserCreatedSagOrchestratorInstanceQuery request, CancellationToken cancellationToken)
    {
        var getUserCreatedSagOrchestratorInstanceResponse = new GetUserCreatedSagOrchestratorInstanceResponse
        {
            UserCreatedSagOrchestratorInstanceResponseDto = new()
        };

        var spec = new GetUserCreatedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

        var userCreatedStateInstance = await _userCreatedSagaStateInstanceRepository.FindAsync(spec);

        if (userCreatedStateInstance == null)
        {
            getUserCreatedSagOrchestratorInstanceResponse.Success = false;
            getUserCreatedSagOrchestratorInstanceResponse.Message = $"You made a Bad Request.";
            getUserCreatedSagOrchestratorInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = null;

            return getUserCreatedSagOrchestratorInstanceResponse;
        }

        getUserCreatedSagOrchestratorInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = _mapper.Map<UserCreatedSagOrchestratorInstanceResponseDto>(userCreatedStateInstance);
        getUserCreatedSagOrchestratorInstanceResponse.Success = true;
        getUserCreatedSagOrchestratorInstanceResponse.Message = $"This resource matched your search";

        return getUserCreatedSagOrchestratorInstanceResponse;
    }
}
