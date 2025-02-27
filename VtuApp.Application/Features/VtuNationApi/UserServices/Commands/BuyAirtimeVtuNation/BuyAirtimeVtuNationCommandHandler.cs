using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using VtuApp.Application.Exceptions;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.Constants;
using VtuApp.Shared.DTO;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyAirtimeVtuNation;

public sealed class BuyAirtimeVtuNationCommandHandler : IRequestHandler<BuyAirtimeVtuNationCommand, BuyAirtimeVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<BuyAirtimeVtuNationCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public BuyAirtimeVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<BuyAirtimeVtuNationCommandHandler> logger,
        IVtuAppRepository<Customer> vtuAppRepository, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService, 
        IMassTransitService massTransitService)
    {
        _getServicesFromVtuNation = getServicesFromVtuNation;
        _logger = logger;
        _vtuAppRepository = vtuAppRepository;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _massTransitService = massTransitService;
    }

    public async Task<BuyAirtimeVtuNationResponse> Handle(BuyAirtimeVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(BuyAirtimeVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        //var buyAirtimeVtuNationResponse = new BuyAirtimeVtuNationResponse();
        //buyAirtimeVtuNationResponse.VtuAirtimePurchaseResponseDto = new();
        var buyAirtimeVtuNationResponse = new BuyAirtimeVtuNationResponse
        {
            VtuAirtimePurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        // this is actually how to parse string into Guid... the Identity User Id == string
        //var userId = new Guid(userExecutingCommand!.Id);
        //var user = await _vtuAppRepository.GetByIdAsync(userId);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date} with request {@Argument}",
                userExecutingCommand?.Email,
                nameof(BuyAirtimeVtuNationCommand),
                DateTimeOffset.UtcNow,
                request.BuyAirtimeRequestVtuNation);

            //throw new ForbiddenAccessException();
            buyAirtimeVtuNationResponse.Success = false;
            buyAirtimeVtuNationResponse.Message = $"Bad Request";
            buyAirtimeVtuNationResponse.VtuAirtimePurchaseResponseDto = null;

            return buyAirtimeVtuNationResponse;
        }

        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.TotalBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = request.BuyAirtimeRequestVtuNation.Amount - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase airtime above current balance: {amountToPurchase} {currentBalance} {time}",
                customer.Email,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buyAirtimeVtuNationResponse.Success = false;
            buyAirtimeVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buyAirtimeVtuNationResponse.VtuAirtimePurchaseResponseDto = null;

            return buyAirtimeVtuNationResponse;
        }

        // At this point it means he/she has sufficient balance
        var networkProvider = request.BuyAirtimeRequestVtuNation.Network.ToLower().ToString() switch
        {
            "airtel" => NetworkProvider.Airtel,
            "mtn" => NetworkProvider.Mtn,
            "glo" => NetworkProvider.Glo,
            "9mobile" => NetworkProvider.NineMobile,

            _ => throw new UnrecognisedNetworkProviderException(request.BuyAirtimeRequestVtuNation.Network)
        }; 

        // deduct customer balance
        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransactionId = customer.AddVtuTransaction(TypeOfTransaction.Airtime, networkProvider, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased airtime with details {TransactionId} {@request} {PriceAfterDiscount} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransactionId,
                request.BuyAirtimeRequestVtuNation,
                priceAfterDiscount,
                initialBalance,
                finalBalance,
                timeOfTransaction   
        );

        // Raise an event
        var customerPurchasedAirtimeVtunationEvent = new CustomerPurchasedAirtimeVtuNationEvent
        {
            CustomerId = customer.CustomerId,
            ApplicationUserId = customer.ApplicationUserId,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            VtuTransactionId = vtuTransactionId,
            AmountPurchased = request.BuyAirtimeRequestVtuNation.Amount,
            PricePaid = priceAfterDiscount,
            NetworkProvider = networkProvider,
            Receiver = request.BuyAirtimeRequestVtuNation.MobileNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedAirtimeVtunationEvent);

        buyAirtimeVtuNationResponse.Success = true;
        buyAirtimeVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuAirtimePurchaseResponseDto = new VtuAirtimePurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Airtime.ToString(),
            Receiver = request.BuyAirtimeRequestVtuNation.MobileNumber,
            Amount = priceAfterDiscount,
            NetworkProvider = networkProvider.ToString(),
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransactionId
        };
        buyAirtimeVtuNationResponse.VtuAirtimePurchaseResponseDto = vtuAirtimePurchaseResponseDto;
        
        return buyAirtimeVtuNationResponse; 
    }
}
