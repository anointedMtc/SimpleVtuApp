using AutoMapper;
using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Common.DTO;
using SagaOrchestrationStateMachines.Common.Specifications;
using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator;

namespace SagaOrchestrationStateMachines.VtuDataOrderedSagaOrchestrator.Helpers.Features.Queries;

public sealed class GetVtuDataOrderedSagaStateInstanceQueryHandler
    : IRequestHandler<GetVtuDataOrderedSagaStateInstanceQuery, GetVtuDataOrderedSagaStateInstanceResponse>
{
    private readonly IRepository<UserCreatedSagaStateInstance> _userCreatedSagaStateInstanceRepository;
    private readonly ILogger<GetVtuDataOrderedSagaStateInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetVtuDataOrderedSagaStateInstanceQueryHandler(
        IRepository<UserCreatedSagaStateInstance> userCreatedSagaStateInstanceRepository, 
        ILogger<GetVtuDataOrderedSagaStateInstanceQueryHandler> logger, 
        IMapper mapper)
    {
        _userCreatedSagaStateInstanceRepository = userCreatedSagaStateInstanceRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetVtuDataOrderedSagaStateInstanceResponse> Handle(GetVtuDataOrderedSagaStateInstanceQuery request, CancellationToken cancellationToken)
    {
        var getVtuDataOrderedSagaStateInstanceResponse = new GetVtuDataOrderedSagaStateInstanceResponse
        {
            VtuDataSagaOrchestratorInstanceResponseDto = new()
        };

        var spec = new GetUserCreatedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

        var vtuDataSagaStateInstance = await _userCreatedSagaStateInstanceRepository.FindAsync(spec);

        if (vtuDataSagaStateInstance == null)
        {
            getVtuDataOrderedSagaStateInstanceResponse.Success = false;
            getVtuDataOrderedSagaStateInstanceResponse.Message = $"You made a Bad Request.";
            getVtuDataOrderedSagaStateInstanceResponse.VtuDataSagaOrchestratorInstanceResponseDto = null;

            return getVtuDataOrderedSagaStateInstanceResponse;
        }

        getVtuDataOrderedSagaStateInstanceResponse.VtuDataSagaOrchestratorInstanceResponseDto = _mapper.Map<VtuDataSagaOrchestratorInstanceResponseDto>(vtuDataSagaStateInstance);
        getVtuDataOrderedSagaStateInstanceResponse.Success = true;
        getVtuDataOrderedSagaStateInstanceResponse.Message = $"This resource matched your search";

        return getVtuDataOrderedSagaStateInstanceResponse;
    }
}
