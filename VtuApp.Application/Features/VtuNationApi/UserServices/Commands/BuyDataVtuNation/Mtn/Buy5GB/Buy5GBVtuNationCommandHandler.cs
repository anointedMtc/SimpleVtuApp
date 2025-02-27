using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.Constants;
using VtuApp.Shared.DTO.VtuNationApi.Constants;
using VtuApp.Shared.DTO;
using VtuApp.Shared.IntegrationEvents;
using SharedKernel.Domain.Interfaces;
using SharedKernel.Application.Interfaces;
using SharedKernel.Application.Exceptions;
using VtuApp.Domain.Interfaces;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy5GB;

internal sealed class Buy5GBVtuNationCommandHandler : IRequestHandler<Buy5GBVtuNationCommand, Buy5GBVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Buy5GBVtuNationCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public Buy5GBVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Buy5GBVtuNationCommandHandler> logger,
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

    public async Task<Buy5GBVtuNationResponse> Handle(Buy5GBVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(Buy5GBVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buy5GBVtuNationResponse = new Buy5GBVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date}",
                userExecutingCommand?.Email,
                nameof(Buy5GBVtuNationCommand),
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy5GBVtuNationResponse.Success = false;
            buy5GBVtuNationResponse.Message = $"Bad Request";
            buy5GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy5GBVtuNationResponse;
        }


        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.TotalBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = MtnDataPriceVtuNation.FiveGB - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase data above current balance: {dataPlan} {discount} {priceAfterDiscount} {currentBalance} {time}",
                customer.Email,
                nameof(MtnDataPriceVtuNation.FiveGB),
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy5GBVtuNationResponse.Success = false;
            buy5GBVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buy5GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy5GBVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransactionId = customer.AddVtuTransaction(TypeOfTransaction.Data, NetworkProvider.Mtn, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransactionId,
                VtuNationDataConstants.MtnFiveGBName,
                MtnDataPriceVtuNation.FiveGB,
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
            DataPlanPurchased = VtuNationDataConstants.MtnFiveGBName,
            NetworkProvider = NetworkProvider.Mtn,
            AmountPurchased = MtnDataPriceVtuNation.FiveGB,
            PricePaid = priceAfterDiscount,
            Receiver = request.PhoneNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);

        buy5GBVtuNationResponse.Success = true;
        buy5GBVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.PhoneNumber,
            DataPlan = VtuNationDataConstants.MtnFiveGBName,
            Label = VtuNationDataConstants.MtnFiveGBLabel,
            Description = VtuNationDataConstants.MtnFiveGBDescription,
            Price = MtnDataPriceVtuNation.FiveGB,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransactionId
        };
        buy5GBVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buy5GBVtuNationResponse;
    }
}
