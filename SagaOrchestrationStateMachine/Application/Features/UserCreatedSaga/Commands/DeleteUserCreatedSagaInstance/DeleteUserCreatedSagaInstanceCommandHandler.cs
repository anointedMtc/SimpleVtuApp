using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SagaOrchestrationStateMachines.Domain.Interfaces;
using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Commands.DeleteUserCreatedSagaInstance;

internal sealed class DeleteUserCreatedSagaInstanceCommandHandler
    : IRequestHandler<DeleteUserCreatedSagaInstanceCommand, DeleteUserCreatedSagaInstanceResponse>
{
    private readonly ISagaStateMachineRepository<UserCreatedSagaStateInstance> _sagaStateMachineRepository;
    private readonly ILogger<DeleteUserCreatedSagaInstanceCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public DeleteUserCreatedSagaInstanceCommandHandler(
        ISagaStateMachineRepository<UserCreatedSagaStateInstance> sagaStateMachineRepository, 
        ILogger<DeleteUserCreatedSagaInstanceCommandHandler> logger, 
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _sagaStateMachineRepository = sagaStateMachineRepository;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<DeleteUserCreatedSagaInstanceResponse> Handle(DeleteUserCreatedSagaInstanceCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(DeleteUserCreatedSagaInstanceCommand),
                request);

            throw new ForbiddenAccessException();
        }

        var deleteUserCreatedSagaInstanceResponse = new DeleteUserCreatedSagaInstanceResponse();

        var userCreatedSagaInstance = await _sagaStateMachineRepository.GetByIdAsync(request.CorrelationId);
       
        if (userCreatedSagaInstance == null)
        {
            deleteUserCreatedSagaInstanceResponse.Success = false;
            deleteUserCreatedSagaInstanceResponse.Message = $"You made a Bad Request";

            return deleteUserCreatedSagaInstanceResponse;
        }

        await _sagaStateMachineRepository.DeleteAsync(userCreatedSagaInstance);

        deleteUserCreatedSagaInstanceResponse.Success = true;
        deleteUserCreatedSagaInstanceResponse.Message = $"204 No-Content";

        return deleteUserCreatedSagaInstanceResponse;
    }
}
