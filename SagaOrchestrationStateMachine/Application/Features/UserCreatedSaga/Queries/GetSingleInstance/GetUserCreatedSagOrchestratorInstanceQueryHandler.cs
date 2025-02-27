using AutoMapper;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Specifications.UserCreatedSaga;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;

public sealed class GetUserCreatedSagOrchestratorInstanceQueryHandler
    : IRequestHandler<GetUserCreatedSagOrchestratorInstanceQuery, GetUserCreatedSagOrchestratorInstanceResponse>
{
    private readonly ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;

    public GetUserCreatedSagOrchestratorInstanceQueryHandler(
        ILogger<GetUserCreatedSagOrchestratorInstanceQueryHandler> logger,
        IMapper mapper, 
        SagaStateMachineDbContext sagaStateMachineDbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
    }

    public async Task<GetUserCreatedSagOrchestratorInstanceResponse> Handle(GetUserCreatedSagOrchestratorInstanceQuery request, CancellationToken cancellationToken)
    {
        var getUserCreatedSagOrchestratorInstanceResponse = new GetUserCreatedSagOrchestratorInstanceResponse
        {
            UserCreatedSagOrchestratorInstanceResponseDto = new()
        };

        var spec = new GetUserCreatedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

        var userCreatedStateInstance = await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);

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


    private IQueryable<UserCreatedSagaStateInstance> ApplySpecification(ISpecification<UserCreatedSagaStateInstance> spec)
    {
        return SpecificationEvaluator<UserCreatedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<UserCreatedSagaStateInstance>().AsQueryable().AsNoTracking(), spec);
    }

}
