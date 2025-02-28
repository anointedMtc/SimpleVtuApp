using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.Commands.TransferVtuBonusToMainWallet;

internal sealed class TransferVtuBonusToMainWalletCommandHandler
    : IRequestHandler<TransferVtuBonusToMainWalletCommand, TransferVtuBonusToMainWalletResponse>
{
    private readonly ILogger<TransferVtuBonusToMainWalletCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public TransferVtuBonusToMainWalletCommandHandler(
        ILogger<TransferVtuBonusToMainWalletCommandHandler> logger, 
        IVtuAppRepository<Customer> vtuAppRepository, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService, 
        IMassTransitService massTransitService)
    {
        _logger = logger;
        _vtuAppRepository = vtuAppRepository;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _massTransitService = massTransitService;
    }

    public async Task<TransferVtuBonusToMainWalletResponse> Handle(TransferVtuBonusToMainWalletCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(TransferVtuBonusToMainWalletCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var transferVtuBonusToMainWalletResponse = new TransferVtuBonusToMainWalletResponse();

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date} with request {@Argument}",
                userExecutingCommand?.Email,
                nameof(TransferVtuBonusToMainWalletCommand),
                DateTimeOffset.UtcNow,
                request.AmountToTransfer);

            throw new ForbiddenAccessException();
        }

        // Warning: The same entity is being tracked as different entity types 'VtuBonusTransfer.InitialBalance#VtuAmount' and 'Customer.VtuBonusBalance#VtuAmount' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
        var initialBonusBalance = customer.VtuBonusBalance;
        if (initialBonusBalance < request.AmountToTransfer)
        {
            _logger.LogInformation("User with Id {userId} tried to transfer bonus amount to wallet that is greater than bonus balance: {amountToTransfer}, {currentBalance}, {time}",
                userExecutingCommand?.Email,
                request.AmountToTransfer,
                initialBonusBalance,
                DateTimeOffset.UtcNow);

            transferVtuBonusToMainWalletResponse.Success = false;
            transferVtuBonusToMainWalletResponse.Message = $"You do not have sufficient Bonus Balance to complete your request";

            return transferVtuBonusToMainWalletResponse;
        }

        var finalBonusBalance = initialBonusBalance - request.AmountToTransfer;
        var timeOfTransaction = DateTimeOffset.UtcNow;


        // Warning: The same entity is being tracked as different entity types 'VtuBonusTransfer.InitialBalance#VtuAmount' and 'Customer.VtuBonusBalance#VtuAmount' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
        var vtuBonusTransfer = customer.DeductFromBonusBalance(request.AmountToTransfer, $"Transfer_to_main_Wallet");
        await _vtuAppRepository.UpdateAsync(customer);

        _logger.LogInformation("Successfully performed and updated customer with Id {customerId} for the following transactions: {operation1} {operation2} {vtuTransferId} {time}",
            customer.Email,
            "DeductFromBonusBalance",
            "AddVtuBonusTransfers",
            vtuBonusTransfer.Id,
            DateTimeOffset.UtcNow);

        await _massTransitService.Publish(new VtuBonusTransferredToWalletEvent(
            customer.ApplicationUserId,
            customer.Email,
            customer.FirstName,
            request.AmountToTransfer,
            timeOfTransaction,
            vtuBonusTransfer.Id,
            // I am doing it this way because of the tracking warning message/issue above...
            initialBonusBalance,
            //customer.VtuBonusBalance + request.AmountToTransfer,  // this was the initialBonusBalance because we have already deducted by now
            finalBonusBalance));

        _logger.LogInformation("Successfully published {typeOfEvent} from {nameOfPublisher} for customer {customerId} with transaction Id {vtuBonusTransferId} at {time}",
            nameof(VtuBonusTransferredToWalletEvent),
            nameof(TransferVtuBonusToMainWalletCommand),
            customer.Email,
            vtuBonusTransfer.Id,
            DateTimeOffset.UtcNow
        );


        transferVtuBonusToMainWalletResponse.Success = true;
        transferVtuBonusToMainWalletResponse.Message = $"Successfully transfered {request.AmountToTransfer} to your main wallet!";

        return transferVtuBonusToMainWalletResponse;
    }
}
