using AutoMapper;
using DomainSharedKernel.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using InfrastructureSharedKernel.SpecificationHelper;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrationStateMachines.Shared.DTO;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Domain.Specifications.VtuAirtimeSaga;
using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using MassTransit.EntityFrameworkCoreIntegration;
using SagaOrchestrationStateMachines.Domain.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;

public sealed class GetVtuAirtimeOrderedSagaStateInstanceQueryHandler
    : IRequestHandler<GetVtuAirtimeOrderedSagaStateInstanceQuery, GetVtuAirtimeOrderedSagaStateInstanceResponse>
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;
    private readonly ILogger<GetVtuAirtimeOrderedSagaStateInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ISagaStateMachineRepository<VtuAirtimeOrderedSagaStateInstance> _sagaStateMachineRepository;

    public GetVtuAirtimeOrderedSagaStateInstanceQueryHandler(
        SagaStateMachineDbContext sagaStateMachineDbContext,
        ILogger<GetVtuAirtimeOrderedSagaStateInstanceQueryHandler> logger,
        IMapper mapper, 
        ISagaStateMachineRepository<VtuAirtimeOrderedSagaStateInstance> sagaStateMachineRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
        _sagaStateMachineRepository = sagaStateMachineRepository;
    }

    public async Task<GetVtuAirtimeOrderedSagaStateInstanceResponse> Handle(GetVtuAirtimeOrderedSagaStateInstanceQuery request, CancellationToken cancellationToken)
    {
        var getVtuAirtimeOrderedSagaStateInstanceResponse = new GetVtuAirtimeOrderedSagaStateInstanceResponse
        {
            VtuAirtimeSagaOrchestratorInstanceResponseDto = new()
        };

        var spec = new GetVtuAirtimeOrderedSagaOrchestratorInstanceByCorrelationId(request.CorrelationId);

        // Always remember that you can choose to set the AsNoTracking here... before FirstOrDefault
        //var vtuAirtimeSagaStateInstance = await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);

        var vtuAirtimeSagaStateInstance = await _sagaStateMachineRepository.FindAsync(spec);

        if (vtuAirtimeSagaStateInstance == null)
        {
            getVtuAirtimeOrderedSagaStateInstanceResponse.Success = false;
            getVtuAirtimeOrderedSagaStateInstanceResponse.Message = $"You made a Bad Request.";
            getVtuAirtimeOrderedSagaStateInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = null;

            return getVtuAirtimeOrderedSagaStateInstanceResponse;
        }

        getVtuAirtimeOrderedSagaStateInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = _mapper.Map<VtuAirtimeSagaOrchestratorInstanceResponseDto>(vtuAirtimeSagaStateInstance);
        getVtuAirtimeOrderedSagaStateInstanceResponse.Success = true;
        getVtuAirtimeOrderedSagaStateInstanceResponse.Message = $"This resource matched your search";

        return getVtuAirtimeOrderedSagaStateInstanceResponse;
    }



    private IQueryable<VtuAirtimeOrderedSagaStateInstance> ApplySpecification(ISpecification<VtuAirtimeOrderedSagaStateInstance> spec)
    {
        // using the DbSet<T> like this would also give you the same thing... however it is like accessing Data from the backstage and you have lost all the extra good stuff that DbContext gives like tracking changes and disposing resources etc...etc...
        //return SpecificationEvaluator<UserCreatedSagaStateInstance>.GetQuery(_dbSetUserCreatedSagaStateInstance.AsQueryable().AsNoTracking(), spec);

        // this is the preferred approcach - using DbContext
        return SpecificationEvaluator<VtuAirtimeOrderedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<VtuAirtimeOrderedSagaStateInstance>().AsQueryable().AsNoTracking(), spec);

    }

}
