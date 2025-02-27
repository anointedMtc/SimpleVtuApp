using AutoMapper;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Specifications.VtuDataSaga;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;

public sealed class GetVtuDataOrderedSagaStateInstanceQueryHandler
    : IRequestHandler<GetVtuDataOrderedSagaStateInstanceQuery, GetVtuDataOrderedSagaStateInstanceResponse>
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;
    private readonly ILogger<GetVtuDataOrderedSagaStateInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetVtuDataOrderedSagaStateInstanceQueryHandler(
        SagaStateMachineDbContext sagaStateMachineDbContext,
        ILogger<GetVtuDataOrderedSagaStateInstanceQueryHandler> logger,
        IMapper mapper)
    {
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetVtuDataOrderedSagaStateInstanceResponse> Handle(GetVtuDataOrderedSagaStateInstanceQuery request, CancellationToken cancellationToken)
    {
        var getVtuDataOrderedSagaStateInstanceResponse = new GetVtuDataOrderedSagaStateInstanceResponse
        {
            VtuDataSagaOrchestratorInstanceResponseDto = new()
        };

        var spec = new GetVtuDataOrderedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

        // Always remember that you can choose to set the AsNoTracking here... before FirstOrDefault
        var vtuDataSagaStateInstance = await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);

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



    private IQueryable<VtuDataOrderedSagaStateInstance> ApplySpecification(ISpecification<VtuDataOrderedSagaStateInstance> spec)
    {
        return SpecificationEvaluator<VtuDataOrderedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<VtuDataOrderedSagaStateInstance>().AsQueryable().AsNoTracking(), spec);
    }

}
