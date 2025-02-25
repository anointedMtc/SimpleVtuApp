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

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy1GB;

internal sealed class Buy1GBVtuNationCommandHandler : IRequestHandler<Buy1GBVtuNationCommand, Buy1GBVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Buy1GBVtuNationCommandHandler> _logger;
    private readonly IRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public Buy1GBVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Buy1GBVtuNationCommandHandler> logger, 
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

    public async Task<Buy1GBVtuNationResponse> Handle(Buy1GBVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(Buy1GBVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buy1GBVtuNationResponse = new Buy1GBVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date}",
                userExecutingCommand?.Email,
                nameof(Buy1GBVtuNationCommand),
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy1GBVtuNationResponse.Success = false;
            buy1GBVtuNationResponse.Message = $"Bad Request";
            buy1GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy1GBVtuNationResponse;
        }


        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.TotalBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = MtnDataPriceVtuNation.OneGB - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase data above current balance: {dataPlan} {discount} {priceAfterDiscount} {currentBalance} {time}",
                customer.Email,
                nameof(MtnDataPriceVtuNation.OneGB),
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy1GBVtuNationResponse.Success = false;
            buy1GBVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buy1GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy1GBVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransactionId = customer.AddVtuTransaction(TypeOfTransaction.Data, NetworkProvider.Mtn, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransactionId,
                VtuNationDataConstants.MtnOneGBName,
                MtnDataPriceVtuNation.OneGB,
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
            DataPlanPurchased = VtuNationDataConstants.MtnOneGBName,
            NetworkProvider = NetworkProvider.Mtn,
            AmountPurchased = MtnDataPriceVtuNation.OneGB,
            PricePaid = priceAfterDiscount,
            Receiver = request.PhoneNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);

        buy1GBVtuNationResponse.Success = true;
        buy1GBVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.PhoneNumber,
            DataPlan = VtuNationDataConstants.MtnOneGBName,
            Label = VtuNationDataConstants.MtnOneGBLabel,
            Description = VtuNationDataConstants.MtnOneGBDescription,
            Price = MtnDataPriceVtuNation.OneGB,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransactionId
        };
        buy1GBVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buy1GBVtuNationResponse;
    }
}
