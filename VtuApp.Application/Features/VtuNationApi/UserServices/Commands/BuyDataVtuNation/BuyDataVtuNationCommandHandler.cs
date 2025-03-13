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
using VtuApp.Shared.DTO.VtuNationApi.Constants;
using VtuApp.Shared.IntegrationEvents;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation;

internal sealed class BuyDataVtuNationCommandHandler : IRequestHandler<BuyDataVtuNationCommand, BuyDataVtuNationResponse>
{
    private readonly IGetServicesFromVtuNation _getServicesFromVtuNation;
    private readonly ILogger<BuyDataVtuNationCommandHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMassTransitService _massTransitService;

    public BuyDataVtuNationCommandHandler(IGetServicesFromVtuNation getServicesFromVtuNation, 
        ILogger<BuyDataVtuNationCommandHandler> logger,
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

    public async Task<BuyDataVtuNationResponse> Handle(BuyDataVtuNationCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Read))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                typeof(BuyDataVtuNationCommand).Name,
                request);

            throw new ForbiddenAccessException();
        }

        var buyDataVtuNationResponse = new BuyDataVtuNationResponse
        {
            VtuDataPurchaseResponseDto = new()
        };

        var spec = new GetCustomerByEmailSpecification(userExecutingCommand!.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            _logger.LogWarning("Non existing user {name} tried to access resource {type} at {date} with request {@Argument}",
                userExecutingCommand?.Email,
                nameof(BuyDataVtuNationCommand),
                DateTimeOffset.UtcNow,
                request.BuyDataRequestVtuNation);

            //throw new ForbiddenAccessException();
            buyDataVtuNationResponse.Success = false;
            buyDataVtuNationResponse.Message = $"Bad Request";
            buyDataVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buyDataVtuNationResponse;
        }

        // figure out network provider 
        var networkProvider = request.BuyDataRequestVtuNation.Network.ToLower() switch
        {
            "airtel" => NetworkProvider.Airtel,
            "mtn" => NetworkProvider.Mtn,
            "glo" => NetworkProvider.Glo,
            "9mobile" => NetworkProvider.NineMobile,

            _ => throw new UnrecognisedNetworkProviderException(request.BuyDataRequestVtuNation.Network)
        };
        // figure out data plan
        string chosenDataPlan = string.Empty;
        decimal chosenDataPlanPrice = 0;
        string chosenDataPlanDescription = string.Empty;
        string chosenDataPlanLabel = string.Empty;

        // MTN
        if (networkProvider == NetworkProvider.Mtn)
        {
            var dataPlan = request.BuyDataRequestVtuNation.DataPlan.ToUpper() switch
            {
                "500MB" => MtnDataPlan.FiveHundredMB,
                "1GB" => MtnDataPlan.OneGB,
                "2GB" => MtnDataPlan.TwoGB,
                "3GB" => MtnDataPlan.ThreeGB,
                "5GB" => MtnDataPlan.FiveGB,
                "10GB" => MtnDataPlan.TenGB,

                _ => throw new UnrecognisedDataPlanException(request.BuyDataRequestVtuNation.DataPlan)
            };

            chosenDataPlan = dataPlan.ToString();

            chosenDataPlanPrice = chosenDataPlan.ToLower() switch
            {
                "fivehundredmb" => 170,
                "onegb" => 280,
                "twogb" => 560,
                "threegb" => 840,
                "fivegb" => 1400,
                "tengb" => 2800,

                _ => throw new UnrecognisedDataPlanPriceException(chosenDataPlan)
            };
        }
        // AIRTEL
        if (networkProvider == NetworkProvider.Airtel)
        {
            var dataPlan = request.BuyDataRequestVtuNation.DataPlan.ToUpper() switch
            {
                "500MB" => AirtelDataPlan.FiveHundredMB,
                "1GB" => AirtelDataPlan.OneGB,
                "2GB" => AirtelDataPlan.TwoGB,
                "3GB" => AirtelDataPlan.ThreeGB,
                "5GB" => AirtelDataPlan.FiveGB,
                "10GB" => AirtelDataPlan.TenGB,

                _ => throw new UnrecognisedDataPlanException(request.BuyDataRequestVtuNation.DataPlan)
            };

            chosenDataPlan = dataPlan.ToString();

            chosenDataPlanPrice = chosenDataPlan.ToLower() switch
            {
                "fivehundredmb" => 170,
                "onegb" => 280,
                "twogb" => 560,
                "threegb" => 840,
                "fivegb" => 1400,
                "tengb" => 2800,

                _ => throw new UnrecognisedDataPlanPriceException(chosenDataPlan)
            };
        }
        // GLO
        if (networkProvider == NetworkProvider.Glo)
        {
            var dataPlan = request.BuyDataRequestVtuNation.DataPlan.ToUpper() switch
            {
                "500MB" => GloDataPlan.FiveHundredMB,
                "1GB" => GloDataPlan.OneGB,
                "2GB" => GloDataPlan.TwoGB,
                "3GB" => GloDataPlan.ThreeGB,
                "5GB" => GloDataPlan.FiveGB,
                "10GB" => GloDataPlan.TenGB,

                _ => throw new UnrecognisedDataPlanException(request.BuyDataRequestVtuNation.DataPlan)
            };

            chosenDataPlan = dataPlan.ToString();

            chosenDataPlanPrice = chosenDataPlan.ToLower() switch
            {
                "fivehundredmb" => 170,
                "onegb" => 280,
                "twogb" => 560,
                "threegb" => 840,
                "fivegb" => 1400,
                "tengb" => 2800,

                _ => throw new UnrecognisedDataPlanPriceException(chosenDataPlan)
            };
        }
        // 9MOBILE
        if (networkProvider == NetworkProvider.NineMobile)
        {
            var dataPlan = request.BuyDataRequestVtuNation.DataPlan.ToUpper() switch
            {
                "500MB" => NineMobileDataPlan.FiveHundredMB,
                "1GB" => NineMobileDataPlan.OneGB,
                "2GB" => NineMobileDataPlan.TwoGB,
                "3GB" => NineMobileDataPlan.ThreeGB,
                "5GB" => NineMobileDataPlan.FiveGB,
                "10GB" => NineMobileDataPlan.TenGB,

                _ => throw new UnrecognisedDataPlanException(request.BuyDataRequestVtuNation.DataPlan)
            };

            chosenDataPlan = dataPlan.ToString();

            chosenDataPlanPrice = chosenDataPlan.ToLower() switch
            {
                "fivehundredmb" => 170,
                "onegb" => 280,
                "twogb" => 560,
                "threegb" => 840,
                "fivegb" => 1400,
                "tengb" => 2800,

                _ => throw new UnrecognisedDataPlanPriceException(chosenDataPlan)
            };
        }

        var initialBalance = customer.MainBalance;
        decimal discount = 0;
        decimal priceAfterDiscount = chosenDataPlanPrice - discount;

        if (!customer.CanBuy(priceAfterDiscount))
        {
            _logger.LogWarning("User with Id {name} tried to purchase dataPlan {TypeOfDataPlan} of network Provider {NetworkProvider} above his/her current balance: {amountToPurchase} {discount} {priceAfterdiscount} {currentBalance} {time}",
                customer.Email,
                chosenDataPlan,
                networkProvider,
                chosenDataPlanPrice,
                discount,
                priceAfterDiscount,
                initialBalance,
                DateTimeOffset.UtcNow
            );

            //throw new ForbiddenAccessException();
            buyDataVtuNationResponse.Success = false;
            buyDataVtuNationResponse.Message = $"Insufficient Funds. Please Credit your wallet and try again";
            buyDataVtuNationResponse.VtuDataPurchaseResponseDto = null;

            return buyDataVtuNationResponse;
        }

        customer.DeductFromCustomerBalance(priceAfterDiscount);

        var timeOfTransaction = DateTimeOffset.UtcNow;

        var vtuTransaction = customer.AddVtuTransaction(TypeOfTransaction.Data, networkProvider, priceAfterDiscount, timeOfTransaction, Status.Pending, discount);

        await _vtuAppRepository.UpdateAsync(customer);

        var finalBalance = initialBalance - priceAfterDiscount;

        _logger.LogInformation("User with Id {name} successfully purchased data with details {TransactionId} {networkProvider} {dataPlan} {dataPrice} {discount} {priceAfterDiscount} {PhoneNumber} {InitialBalance} {FinalBalance} {time}",
                customer.Email,
                vtuTransaction.Id,
                networkProvider,
                chosenDataPlan,
                chosenDataPlanPrice,
                discount,
                priceAfterDiscount,
                request.BuyDataRequestVtuNation.MobileNumber,
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
            DataPlanPurchased = chosenDataPlan,
            NetworkProvider = networkProvider,
            AmountPurchased = chosenDataPlanPrice,
            PricePaid = priceAfterDiscount,
            Receiver = request.BuyDataRequestVtuNation.MobileNumber,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
        };
        await _massTransitService.Publish(customerPurchasedDataVtuNationEvent);


        // label - JUST PUT IN EVERY POSSIBLE VALUE FROM EVERY POSSIBLE NETWORK
        chosenDataPlanLabel = chosenDataPlan switch
        {
            "500MB" => "500MB 30 Days",
            "1GB" => "1GB 30 Days",
            "2GB" => "2GB 30 Days",
            "3GB" => "2GB 30 Days",
            "5GB" => "5GB 30 Days SME",
            "10GB" => "10GB 30 Days SME",

            _ => throw new UnrecognisedDataPlanException(chosenDataPlan)
        };
        // description - JUST PUT IN EVERY POSSIBLE VALUE FROM EVERY POSSIBLE NETWORK
        chosenDataPlanDescription = chosenDataPlan switch
        {
            "500MB" => "30 Days",
            "1GB" => "30 Days",
            "2GB" => "30 Days",
            "3GB" => "3GB Monthly SME",
            "5GB" => "5GB Monthly SME",
            "10GB" => "10GB 30 Days SME",

            _ => throw new UnrecognisedDataPlanException(chosenDataPlan)
        };

        buyDataVtuNationResponse.Success = true;
        buyDataVtuNationResponse.Message = $"Your purchase order was recieved and is being processed. Details will be sent to your email shortly";
        var vtuDataPurchaseResponseDto = new VtuDataPurchaseResponseDto
        {
            TransactionType = TypeOfTransaction.Data.ToString(),
            NetworkProvider = NetworkProvider.Mtn.ToString(),
            Receiver = request.BuyDataRequestVtuNation.MobileNumber,
            DataPlan = chosenDataPlan,
            Label = chosenDataPlanLabel,
            Description = chosenDataPlanDescription,
            Price = chosenDataPlanPrice,
            Discount = 0,
            PriceAfterDiscount = priceAfterDiscount,
            Sender = customer.PhoneNumber,
            InitialBalance = initialBalance,
            FinalBalance = finalBalance,
            CreatedAt = timeOfTransaction,
            VtuTransactionId = vtuTransaction.Id
        };
        buyDataVtuNationResponse.VtuDataPurchaseResponseDto = vtuDataPurchaseResponseDto;

        return buyDataVtuNationResponse;
    }
}
