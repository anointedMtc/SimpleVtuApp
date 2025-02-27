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

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy10GB;

internal sealed class Buy10GBVtuNationCommandHandler : IRequestHandler<Buy10GBVtuNationCommand, Buy10GBVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Buy10GBVtuNationCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public Buy10GBVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Buy10GBVtuNationCommandHandler> logger,
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

    public async Task<Buy10GBVtuNationResponse> Handle(Buy10GBVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(Buy10GBVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buy10GBVtuNationResponse = new Buy10GBVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date}",
                userExecutingCommand?.Email,
                nameof(Buy10GBVtuNationCommand),
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy10GBVtuNationResponse.Success = false;
            buy10GBVtuNationResponse.Message = $"Bad Request";
            buy10GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy10GBVtuNationResponse;
        }


        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.TotalBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = MtnDataPriceVtuNation.TenGB - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase data above current balance: {dataPlan} {discount} {priceAfterDiscount} {currentBalance} {time}",
                customer.Email,
                nameof(MtnDataPriceVtuNation.TenGB),
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy10GBVtuNationResponse.Success = false;
            buy10GBVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buy10GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy10GBVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransactionId = customer.AddVtuTransaction(TypeOfTransaction.Data, NetworkProvider.Mtn, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransactionId,
                VtuNationDataConstants.MtnTenGBName,
                MtnDataPriceVtuNation.TenGB,
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
            DataPlanPurchased = VtuNationDataConstants.MtnTenGBName,
            NetworkProvider = NetworkProvider.Mtn,
            AmountPurchased = MtnDataPriceVtuNation.TenGB,
            PricePaid = priceAfterDiscount,
            Receiver = request.PhoneNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);

        buy10GBVtuNationResponse.Success = true;
        buy10GBVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.PhoneNumber,
            DataPlan = VtuNationDataConstants.MtnTenGBName,
            Label = VtuNationDataConstants.MtnTenGBLabel,
            Description = VtuNationDataConstants.MtnTenGBDescription,
            Price = MtnDataPriceVtuNation.FiveGB,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransactionId
        };
        buy10GBVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buy10GBVtuNationResponse;
    }
}
