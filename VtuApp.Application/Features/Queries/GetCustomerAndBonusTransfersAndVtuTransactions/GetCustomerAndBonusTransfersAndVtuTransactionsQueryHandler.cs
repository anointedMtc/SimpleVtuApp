using AutoMapper;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Exceptions;
using SharedKernel.Application.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;
using VtuApp.Domain.Interfaces;
using VtuApp.Domain.Specifications;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

internal sealed class GetCustomerAndBonusTransfersAndVtuTransactionsQueryHandler
    : IRequestHandler<GetCustomerAndBonusTransfersAndVtuTransactionsQuery, GetCustomerAndBonusTransfersAndVtuTransactionsResponse>
{
    private readonly ILogger<GetCustomerAndBonusTransfersAndVtuTransactionsQueryHandler> _logger;
    private readonly IVtuAppRepository<Customer> _vtuAppRepository;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IMapper _mapper;

    public GetCustomerAndBonusTransfersAndVtuTransactionsQueryHandler(
        ILogger<GetCustomerAndBonusTransfersAndVtuTransactionsQueryHandler> logger, 
        IVtuAppRepository<Customer> vtuAppRepository, IUserContext userContext, 
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IMapper mapper)
    {
        _logger = logger;
        _vtuAppRepository = vtuAppRepository;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _mapper = mapper;
    }

    public async Task<GetCustomerAndBonusTransfersAndVtuTransactionsResponse> Handle(GetCustomerAndBonusTransfersAndVtuTransactionsQuery request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand?.Email ?? "Anonymous User",
                nameof(GetCustomerAndBonusTransfersAndVtuTransactionsQuery),
                request);

            throw new ForbiddenAccessException();
        }

        var getCustomerAndBonusTransfersAndVtuTransactionsResponse = new GetCustomerAndBonusTransfersAndVtuTransactionsResponse();

        var spec = new GetCustomerAndBonusTransfersAndVtuTransactionsByEmailSpecification(request.Email);

        var customer = await _vtuAppRepository.FindAsync(spec);
        if (customer == null)
        {
            getCustomerAndBonusTransfersAndVtuTransactionsResponse.Success = false;
            getCustomerAndBonusTransfersAndVtuTransactionsResponse.Message = $"You made a Bad Request";

            return getCustomerAndBonusTransfersAndVtuTransactionsResponse;
        }

        getCustomerAndBonusTransfersAndVtuTransactionsResponse.CustomerDto = _mapper.Map<CustomerDto>(customer);

        getCustomerAndBonusTransfersAndVtuTransactionsResponse.Success = true;
        getCustomerAndBonusTransfersAndVtuTransactionsResponse.Message = $"This resource matched your search";

        return getCustomerAndBonusTransfersAndVtuTransactionsResponse;
    }
}
