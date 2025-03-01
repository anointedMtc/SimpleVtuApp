using AutoMapper;
using Identity.Shared.Constants;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Specifications.VtuDataSaga;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetAllSagaInstance;

internal sealed class GetAllVtuDataSagaInstanceQueryHandler
    : IRequestHandler<GetAllVtuDataSagaInstanceQuery, Pagination<GetAllVtuDataSagaInstanceResponse>>
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;
    private readonly ILogger<GetAllVtuDataSagaInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetAllVtuDataSagaInstanceQueryHandler(
        SagaStateMachineDbContext sagaStateMachineDbContext,
        ILogger<GetAllVtuDataSagaInstanceQueryHandler> logger, IMapper mapper,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<Pagination<GetAllVtuDataSagaInstanceResponse>> Handle(GetAllVtuDataSagaInstanceQuery request, CancellationToken cancellationToken)
    {
        var getAllVtuDataSagaInstanceResponse = new GetAllVtuDataSagaInstanceResponse
        {
            VtuDataSagaOrchestratorInstanceResponseDto = []
        };

        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                nameof(GetAllVtuDataSagaInstanceQuery),
                request);

            //throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");

            getAllVtuDataSagaInstanceResponse.Success = false;
            getAllVtuDataSagaInstanceResponse.Message = $"You are not authorized to access this endpoint.";
            getAllVtuDataSagaInstanceResponse.VtuDataSagaOrchestratorInstanceResponseDto = null;

            return new Pagination<GetAllVtuDataSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllVtuDataSagaInstanceResponse);
        }


        var spec = new GetAllVtuDataSagaOrchestratorInstanceSpecification(request.PaginationFilter);

        var data = SpecificationEvaluator<VtuDataOrderedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<VtuDataOrderedSagaStateInstance>().AsQueryable().AsNoTracking(), spec);
        totalUsers = await SpecificationEvaluator<VtuDataOrderedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<VtuDataOrderedSagaStateInstance>().AsQueryable().AsNoTracking(), spec).CountAsync(cancellationToken);


        getAllVtuDataSagaInstanceResponse.Success = true;
        getAllVtuDataSagaInstanceResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllVtuDataSagaInstanceResponse.VtuDataSagaOrchestratorInstanceResponseDto = _mapper.Map<List<VtuDataSagaOrchestratorInstanceResponseDto>>(data);


        return new Pagination<GetAllVtuDataSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllVtuDataSagaInstanceResponse);
    }
}
