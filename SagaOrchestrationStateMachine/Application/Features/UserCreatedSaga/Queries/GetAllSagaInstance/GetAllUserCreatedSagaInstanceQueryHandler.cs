﻿using AutoMapper;
using Identity.Shared.Constants;
using MassTransit.EntityFrameworkCoreIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Specifications.UserCreatedSaga;
using SagaOrchestrationStateMachines.Infrastructure.Persistence;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Infrastructure.SpecificationHelper;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;

public sealed class GetAllUserCreatedSagaInstanceQueryHandler
    : IRequestHandler<GetAllUserCreatedSagaInstanceQuery, Pagination<GetAllUserCreatedSagaInstanceResponse>>
{
    private readonly SagaStateMachineDbContext _sagaStateMachineDbContext;
    private readonly ILogger<GetAllUserCreatedSagaInstanceQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public GetAllUserCreatedSagaInstanceQueryHandler(
        SagaStateMachineDbContext sagaStateMachineDbContext,
        ILogger<GetAllUserCreatedSagaInstanceQueryHandler> logger,
        IMapper mapper, IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _sagaStateMachineDbContext = sagaStateMachineDbContext;
        _logger = logger;
        _mapper = mapper;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<Pagination<GetAllUserCreatedSagaInstanceResponse>> Handle(GetAllUserCreatedSagaInstanceQuery request, CancellationToken cancellationToken)
    {
        var getAllUserCreatedSagaInstanceResponse = new GetAllUserCreatedSagaInstanceResponse
        {
            UserCreatedSagOrchestratorInstanceResponseDto = []
        };

        int totalUsers = 0;

        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                nameof(GetAllUserCreatedSagaInstanceQuery),
                request);

            //throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");

            getAllUserCreatedSagaInstanceResponse.Success = false;
            getAllUserCreatedSagaInstanceResponse.Message = $"You are not authorized to access this endpoint.";
            getAllUserCreatedSagaInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = null;

            return new Pagination<GetAllUserCreatedSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllUserCreatedSagaInstanceResponse);
        }


        var spec = new GetAllUserCreatedSagaOrchestratorInstanceSpecification(request.PaginationFilter);

        var data = ApplySpecification(spec);
        totalUsers = await ApplySpecification(spec).CountAsync(cancellationToken);


        getAllUserCreatedSagaInstanceResponse.Success = true;
        getAllUserCreatedSagaInstanceResponse.Message = $"your query was successful and this is the list of UserCreatedSagaInstance in {request.PaginationFilter.Sort ?? "Default"} order, matching {request.PaginationFilter.Search ?? "No search or filters"}";
        getAllUserCreatedSagaInstanceResponse.UserCreatedSagOrchestratorInstanceResponseDto = _mapper.Map<List<UserCreatedSagOrchestratorInstanceResponseDto>>(data);


        return new Pagination<GetAllUserCreatedSagaInstanceResponse>(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, totalUsers, getAllUserCreatedSagaInstanceResponse);

    }


    private IQueryable<UserCreatedSagaStateInstance> ApplySpecification(ISpecification<UserCreatedSagaStateInstance> spec)
    {
        return SpecificationEvaluator<UserCreatedSagaStateInstance>.GetQuery(_sagaStateMachineDbContext.Set<UserCreatedSagaStateInstance>().AsQueryable().AsNoTracking(), spec);
    }
}
