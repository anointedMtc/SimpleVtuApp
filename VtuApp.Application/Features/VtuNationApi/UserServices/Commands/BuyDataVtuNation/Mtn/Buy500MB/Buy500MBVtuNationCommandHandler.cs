using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.Constants;
using VtuApp.Shared.DTO;
using VtuApp.Shared.DTO.VtuNationApi.Constants;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy500MB;

internal sealed class Buy500MBVtuNationCommandHandler : IRequestHandler<Buy500MBVtuNationCommand, Buy500MBVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Buy500MBVtuNationCommandHandler> _logger;
    private readonly IRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public Buy500MBVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Buy500MBVtuNationCommandHandler> logger, 
        IRepository<Customer> vtuAppRepository, IUserContext userContext, 
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

    public async Task<Buy500MBVtuNationResponse> Handle(Buy500MBVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(Buy500MBVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buy500MBVtuNationResponse = new Buy500MBVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date}",
                userExecutingCommand?.Email,
                nameof(Buy500MBVtuNationCommand),
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy500MBVtuNationResponse.Success = false;
            buy500MBVtuNationResponse.Message = $"Bad Request";
            buy500MBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy500MBVtuNationResponse;
        }


        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.TotalBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = MtnDataPriceVtuNation.FiveHundredMb - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase data above current balance: {dataPlan} {discount} {priceAfterDiscount} {currentBalance} {time}",
                customer.Email,
                nameof(MtnDataPriceVtuNation.FiveHundredMb),
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy500MBVtuNationResponse.Success = false;
            buy500MBVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buy500MBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy500MBVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransactionId = customer.AddVtuTransaction(TypeOfTransaction.Data, NetworkProvider.Mtn, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransactionId,
                VtuNationDataConstants.MtnFiveHundredMBName,
                MtnDataPriceVtuNation.FiveHundredMb,
                discount,
                priceAfterDiscount,
                request.PhoneNumber,
                initialBalance,
                finalBalance,
                timeOfTransaction
        );

        // Raise an event
        var customerPurchasedDataVtuNationEvent = new CustomerPurchasedDataVtuNationEvent
        {
            CustomerId = customer.CustomerId,
            ApplicationUserId = customer.ApplicationUserId,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            VtuTransactionId = vtuTransactionId,
            DataPlanPurchased = VtuNationDataConstants.MtnFiveHundredMBName,
            NetworkProvider = NetworkProvider.Mtn,
            AmountPurchased = MtnDataPriceVtuNation.FiveHundredMb,
            PricePaid = priceAfterDiscount,
            Receiver = request.PhoneNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);

        buy500MBVtuNationResponse.Success = true;
        buy500MBVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.PhoneNumber,
            DataPlan = VtuNationDataConstants.MtnFiveHundredMBName,
            Label = VtuNationDataConstants.MtnFiveHundredMBLabel,
            Description = VtuNationDataConstants.MtnFiveHundredMBDescription,
            Price = MtnDataPriceVtuNation.FiveHundredMb,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransactionId
        };
        buy500MBVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buy500MBVtuNationResponse;
    }
}
