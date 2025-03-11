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

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy2GB;

internal sealed class Buy2GBVtuNationCommandHandler : IRequestHandler<Buy2GBVtuNationCommand, Buy2GBVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<Buy2GBVtuNationCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public Buy2GBVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<Buy2GBVtuNationCommandHandler> logger,
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

    public async Task<Buy2GBVtuNationResponse> Handle(Buy2GBVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(Buy2GBVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buy2GBVtuNationResponse = new Buy2GBVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date}",
                userExecutingCommand?.Email,
                nameof(Buy2GBVtuNationCommand),
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy2GBVtuNationResponse.Success = false;
            buy2GBVtuNationResponse.Message = $"Bad Request";
            buy2GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy2GBVtuNationResponse;
        }


        // but if customer exists, does he/she have enough sufficient funds to proceed?
        var initialBalance = customer.MainBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = MtnDataPriceVtuNation.TwoGB - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase data above current balance: {dataPlan} {discount} {priceAfterDiscount} {currentBalance} {time}",
                customer.Email,
                nameof(MtnDataPriceVtuNation.TwoGB),
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buy2GBVtuNationResponse.Success = false;
            buy2GBVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buy2GBVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buy2GBVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransaction = customer.AddVtuTransaction(TypeOfTransaction.Data, NetworkProvider.Mtn, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        await _vtuAppRepository.UpdateAsync(customer);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransaction.Id,
                VtuNationDataConstants.MtnTwoGBName,
                MtnDataPriceVtuNation.TwoGB,
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
            VtuTransactionId = vtuTransaction.Id,
            DataPlanPurchased = VtuNationDataConstants.MtnTwoGBName,
            NetworkProvider = NetworkProvider.Mtn,
            AmountPurchased = MtnDataPriceVtuNation.TwoGB,
            PricePaid = priceAfterDiscount, 
            Receiver = request.PhoneNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);

        buy2GBVtuNationResponse.Success = true;
        buy2GBVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.PhoneNumber,
            DataPlan = VtuNationDataConstants.MtnTwoGBName,
            Label = VtuNationDataConstants.MtnTwoGBLabel,
            Description = VtuNationDataConstants.MtnTwoGBDescription,
            Price = MtnDataPriceVtuNation.TwoGB,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransaction.Id
        };
        buy2GBVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buy2GBVtuNationResponse;
    }
}
