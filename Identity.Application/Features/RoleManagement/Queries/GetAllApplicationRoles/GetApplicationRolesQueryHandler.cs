using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Application.Specifications;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Shared.Constants;
using Identity.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.Interfaces;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllApplicationRoles;

public class GetApplicationRolesQueryHandler : IRequestHandler<GetApplicationRolesQuery, Pagination<GetApplicationRolesResponse>>
{
    private readonly IRepository<ApplicationRole> _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetApplicationRolesQueryHandler> _logger;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    private readonly ISpecificationHelperIdentity<ApplicationRole> _specificationHelperIdentity;

    public GetApplicationRolesQueryHandler(IRepository<ApplicationRole> repository,
        IMapper mapper, ILogger<GetApplicationRolesQueryHandler> logger,
        RoleManager<ApplicationRole> roleManager,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext,
        ISpecificationHelperIdentity<ApplicationRole> specificationHelperIdentity)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _roleManager = roleManager;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
        _specificationHelperIdentity = specificationHelperIdentity;
    }

    public async Task<Pagination<GetApplicationRolesResponse>> Handle(GetApplicationRolesQuery request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(GetApplicationRolesQuery),
                request);

            throw new CustomForbiddenException();
        }

        var getApplicationRoleResponse = new GetApplicationRolesResponse();
        var totalRoles = 0;

        var spec = new ApplicationRoleSpecification(request.PaginationFilterAppUser);

        //var data = await _repository.GetAllAsync(spec);
        var data = await _specificationHelperIdentity.GetAllAsync(spec);

        if (!data.Any())
        {
            getApplicationRoleResponse.Message = $"No resource matched your search. Please try a different entry";

            totalRoles = 0;

            _logger.LogError("Error 404. The resorce could not be found for: {RequestName}, with {@SearchParams} at {DateTimeUtc}",
                typeof(GetApplicationRolesQuery),
                request.PaginationFilterAppUser,
                DateTime.UtcNow);

            return new Pagination<GetApplicationRolesResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalRoles, getApplicationRoleResponse);

        }

        //totalRoles = await _repository.CountAsync(spec);
        totalRoles = await _specificationHelperIdentity.CountAsync(spec);

        getApplicationRoleResponse.ApplicationRoleResponseDto = _mapper.Map<List<ApplicationRoleResponseDto>>(data);

        getApplicationRoleResponse.Message = $"your query was successful and this is the list of Application Roles in {request.PaginationFilterAppUser.Sort} order";
        getApplicationRoleResponse.Success = true;


        return new Pagination<GetApplicationRolesResponse>(request.PaginationFilterAppUser.PageNumber, request.PaginationFilterAppUser.PageSize, totalRoles, getApplicationRoleResponse);
    }
}
