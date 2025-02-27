using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Interfaces;
using SagaOrchestrationStateMachines.Domain.Specifications.VtuAirtimeSaga;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuAirtimeSagaInstanceQueryHandler
    : IRequestHandler<GetAllVtuAirtimeSagaInstanceQuery, Pagination<GetAllVtuAirtimeSagaInstanceResponse>>
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;
    private readonly ILogger<GetAllVtuAirtimeSagaInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;
    private readonly ISagaStateMachineRepository<VtuAirtimeOrderedSagaStateInstance> _sagaStateMachineRepository;

    public GetAllVtuAirtimeSagaInstanceQueryHandler(
        SagaStateMachineDbContext sagaStateMachineDbContext,
        ILogger<GetAllVtuAirtimeSagaInstanceQueryHandler> logger,
        IMapper mapper, IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext, 
        ISagaStateMachineRepository<VtuAirtimeOrderedSagaStateInstance> sagaStateMachineRepository)
    {
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
        _sagaStateMachineRepository = sagaStateMachineRepository;
    }

    public async Task<Pagination<GetAllVtuAirtimeSagaInstanceResponse>> Handle(GetAllVtuAirtimeSagaInstanceQuery request, CancellationToken cancellationToken)
    {
        var getAllVtuAirtimeSagaInstanceResponse = new GetAllVtuAirtimeSagaInstanceResponse
        {
            VtuAirtimeSagaOrchestratorInstanceResponseDto = []
        };

        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                nameof(GetAllVtuAirtimeSagaInstanceQuery),
                request);

            //throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");

            getAllVtuAirtimeSagaInstanceResponse.Success = false;
            getAllVtuAirtimeSagaInstanceResponse.Message = $"You made a Bad Request.";
            getAllVtuAirtimeSagaInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = null;

            return new Pagination<GetAllVtuAirtimeSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllVtuAirtimeSagaInstanceResponse);
        }


        var spec = new GetAllVtuAirtimeSagaOrchestratorInstanceSpecification(request.PaginationFilter);

        var data = await _sagaStateMachineRepository.GetAllAsync(spec);
        totalUsers = await _sagaStateMachineRepository.CountAsync(spec);

        getAllVtuAirtimeSagaInstanceResponse.Success = true;
        getAllVtuAirtimeSagaInstanceResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllVtuAirtimeSagaInstanceResponse.VtuAirtimeSagaOrchestratorInstanceResponseDto = _mapper.Map<List<VtuAirtimeSagaOrchestratorInstanceResponseDto>>(data);


        return new Pagination<GetAllVtuAirtimeSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllVtuAirtimeSagaInstanceResponse);
    }
}
