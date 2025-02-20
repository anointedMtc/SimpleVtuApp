namespace Identity.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class ManageRoleController : ApiBaseController
{

    // READS
    [HttpGet("get-All-Application-Roles")]
    public async Task<ActionResult<Pagination<GetApplicationRolesResponse>>> GetApplicationRoles([FromQuery] PaginationFilter paginationFilterAppUser)
    {
        var result = await Mediator.Send(new GetApplicationRolesQuery(paginationFilterAppUser));

        return Ok(result);
    }


    [HttpGet("get-role-by-id/{roleId}")]
    public async Task<ActionResult<GetRoleByIdResponse>> GetRoleById(Guid roleId)
    {
        var result = await Mediator.Send(new GetRoleByIdQuery() { RoleId = roleId });

        return Ok(result);
    }


    [HttpGet("get-role-by-name/{name}")]
    public async Task<ActionResult<GetRoleByNameResponse>> GetRoleByName(string name)
    {
        var result = await Mediator.Send(new GetRoleByNameQuery() { Name = name });

        return Ok(result);
    }



    // WRITES

    [HttpPost("add-application-role")]
    public async Task<ActionResult<AddApplicationRoleResponse>> AddApplicationRole([FromBody] AddApplicationRoleRequestDto addApplicationRoleRequestDto)
    {
        var result = await Mediator.Send(new AddApplicationRoleCommand() { AddApplicationRoleRequestDto = addApplicationRoleRequestDto });

        return Ok(result);
    }


    [HttpPut("update-app-role/{roleId}")]
    public async Task<ActionResult<UpdateApplicationRoleResponse>> UpdateApplicationRole(Guid roleId, [FromBody] UpdateApplicationRoleRequestDto updateApplicationRoleRequestDto)
    {
        var result = await Mediator.Send(new UpdateApplicationRoleCommand() { RoleId = roleId, UpdateApplicationRoleRequestDto = updateApplicationRoleRequestDto });

        return Ok(result);
    }


    [HttpDelete("delete-role/{roleId}")]
    public async Task<ActionResult<DeleteApplicationRoleResponse>> DeleteApplicationRole(Guid roleId)
    {
        var result = await Mediator.Send(new DeleteApplicationRoleCommand() { RoleId = roleId });

        return Ok(result);
    }


    // Claims
    [HttpGet("get-all-claims-for-a-role/{roleId}")]
    public async Task<ActionResult<GetAllClaimsForARoleResponse>> GetAllClaimsForARole(Guid roleId)
    {
        var result = await Mediator.Send(new GetAllClaimsForARoleQuery() { RoleId = roleId });

        return Ok(result);
    }


    [HttpPost("add-role-claim/{roleId}")]
    public async Task<ActionResult<AddRoleClaimResponse>> AddRoleClaim(Guid roleId, [FromBody] AddRoleClaimRequestDto addRoleClaimRequestDto)
    {
        var result = await Mediator.Send(new AddRoleClaimCommand() { AppRoleId = roleId, AddRoleClaimRequestDto = addRoleClaimRequestDto });

        return Ok(result);
    }


    [HttpDelete("remove-role-claim/{roleId}")]
    public async Task<ActionResult<RemoveRoleClaimResponse>> RemoveRoleClaim(Guid roleId, [FromBody] RemoveRoleClaimRequestDto removeRoleClaimRequestDto)
    {
        var result = await Mediator.Send(new RemoveRoleClaimCommand() { AppRoleId = roleId, RemoveRoleClaimRequestDto = removeRoleClaimRequestDto });

        return Ok(result);
    }

}
