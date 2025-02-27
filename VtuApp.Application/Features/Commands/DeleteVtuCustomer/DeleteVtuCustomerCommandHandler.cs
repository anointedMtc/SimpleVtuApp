using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;

namespace VtuApp.Application.Features.Commands.DeleteVtuCustomer;

internal sealed class DeleteVtuCustomerCommandHandler : IRequestHandler<DeleteVtuCustomerCommand, DeleteVtuCustomerResponse>
{
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly ILogger<DeleteVtuCustomerCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public DeleteVtuCustomerCommandHandler(IVtuAppRepository<Customer> vtuAppRepository, 
        ILogger<DeleteVtuCustomerCommandHandler> logger, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _vtuAppRepository = vtuAppRepository;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }

    public async Task<DeleteVtuCustomerResponse> Handle(DeleteVtuCustomerCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.GodsEyeOnly))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(DeleteVtuCustomerCommand),
                request);

            throw new ForbiddenAccessException();
        }

        var deleteVtuCustomerResponse = new DeleteVtuCustomerResponse();

        var spec = new GetCustomerByEmailSpecification(request.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            deleteVtuCustomerResponse.Success = false;
            deleteVtuCustomerResponse.Message = $"You made a Bad Request";

            return deleteVtuCustomerResponse;
        }

        await _vtuAppRepository.DeleteAsync(customer);

        deleteVtuCustomerResponse.Success = true;
        deleteVtuCustomerResponse.Message = $"204 No-Content";

        return deleteVtuCustomerResponse;
    }
}
