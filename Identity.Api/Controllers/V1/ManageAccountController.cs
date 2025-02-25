using SharedKernel.Application.HelperClasses;

namespace Identity.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class ManageAccountController : ApiBaseController
{
    // READS

    [HttpGet("get-all-application-users")]
    public async Task<ActionResult<Pagination<GetApplicationUsersResponse>>> GetApplicationUsers([FromQuery] PaginationFilter paginationFilterAppUser)
    {
        var result = await Mediator.Send(new GetApplicationUsersQuery(paginationFilterAppUser));

        return Ok(result);
    }


    [HttpGet("get-all-users-in-a-role")]
    public async Task<ActionResult<Pagination<GetAllUsersInARoleResponse>>> GetAllUsersInARole([FromQuery] PaginationFilter paginationFilterAppUser)
    {
        var result = await Mediator.Send(new GetAllUsersInARoleQuery(paginationFilterAppUser));

        return Ok(result);
    }


    [HttpGet("get-all-users-for-a-claim")]
    public async Task<ActionResult<Pagination<GetAllUsersForAClaimResponse>>> GetAllUsersForAClaim([FromQuery] PaginationFilter paginationFilterAppUser, [FromQuery] GetAllUsersForAClaimRequestDto getAllUsersForAClaimRequestDto)
    {
        var result = await Mediator.Send(new GetAllUsersForAClaimQuery(paginationFilterAppUser) { GetAllUsersForAClaimRequestDto = getAllUsersForAClaimRequestDto });

        return Ok(result);
    }


    [HttpGet("get-user-by-email/{email}")]
    public async Task<ActionResult<GetUserByEmailResponse>> GetUserByEmail(string email)
    {
        var result = await Mediator.Send(new GetUserByEmailQuery() { Email = email });

        return Ok(result);
    }


    [HttpGet("get-user-by-id/{userId}")]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserById(Guid userId)
    {
        var result = await Mediator.Send(new GetUserByIdQuery() { UserId = userId });

        return Ok(result);
    }


    [HttpGet("get-user-claims/{userId}")]
    public async Task<ActionResult<GetUserClaimsResponse>> GetUserClaims(Guid userId)
    {
        var result = await Mediator.Send(new GetUserClaimsQuery() { UserId = userId });

        return Ok(result);
    }




    // WRITES

    // CLAIMS
    [HttpPost("add-user-claim/{userId}")]
    public async Task<ActionResult<AddUserClaimResponse>> AddUserClaim(Guid userId, [FromBody] AddUserClaimRequestDto addUserClaimRequestDto)
    {
        var result = await Mediator.Send(new AddUserClaimCommand() { UserId = userId, AddUserClaimRequestDto = addUserClaimRequestDto });

        return Ok(result);
    }


    [HttpPut("update-user-claim/{userId}")]
    public async Task<ActionResult<UpdateUserClaimResponse>> UpdateUserClaim(Guid userId, [FromBody] UpdateUserClaimRequestDto updateUserClaimRequestDto)
    {
        var result = await Mediator.Send(new UpdateUserClaimCommand() { UserId = userId, UpdateUserClaimRequestDto = updateUserClaimRequestDto });

        return Ok(result);
    }


    [HttpDelete("remove-user-claim /{userId}")]
    public async Task<ActionResult<RemoveUserClaimResponse>> RemoveUserClaim(Guid userId, [FromBody] RemoveUserClaimRequestDto removeUserClaimRequestDto)
    {
        var result = await Mediator.Send(new RemoveUserClaimCommand() { UserId = userId, RemoveUserClaimRequestDto = removeUserClaimRequestDto });

        return Ok(result);
    }


    [HttpDelete("delete-all-claims-for-a-user/{userId}")]
    public async Task<ActionResult<DeleteAllClaimsForAUserResponse>> DeleteAllClaimsForAUser(Guid userId)
    {
        var result = await Mediator.Send(new DeleteAllClaimsForAUserCommand() { UserId = userId });

        return Ok(result);
    }


    // ROLES
    [HttpPost("assign-user-role")]
    public async Task<ActionResult<AssignUserRoleResponse>> AssignUserRole([FromBody] AssignUserRoleRequestDto assignUserRoleRequestDto)
    {
        var result = await Mediator.Send(new AssignUserRoleCommand() { AssignUserRoleRequestDto = assignUserRoleRequestDto });

        return Ok(result);
    }


    [HttpPost("unassign-user-role")]
    public async Task<ActionResult<UnassignUserRoleResponse>> UnassignUserRole([FromBody] UnassignUserRoleRequestDto unassignUserRoleRequestDto)
    {
        var result = await Mediator.Send(new UnassignUserRoleCommand() { UnassignUserRoleRequestDto = unassignUserRoleRequestDto });

        return Ok(result);
    }


    // LOCK AND UNLOCK
    //[HttpPost("lockoutuser/{userId}")]
    [HttpPost("lock-out-user")]
    public async Task<ActionResult<LockOutUserResponse>> LockOutUser(Guid userId)
    {
        var result = await Mediator.Send(new LockOutUserCommand() { UserId = userId });

        return Ok(result);
    }


    [HttpPost("unlock-user")]
    public async Task<ActionResult<UnlockUserResponse>> UnlockUser(Guid userId)
    {
        var result = await Mediator.Send(new UnlockUserCommand() { UserId = userId });

        return Ok(result);
    }


    [HttpPost("revoke-refresh-token")]
    public async Task<ActionResult<RevokeRefreshTokenResponse>> RevokeRefreshToken([FromBody] RevokeRefreshTokenRequestDto revokeRefreshTokenRequestDto)
    {
        var result = await Mediator.Send(new RevokeRefreshTokenCommand() { RevokeRefreshTokenRequestDto = revokeRefreshTokenRequestDto });

        return Ok(result);
    }


    // Update User
    [HttpPut("updateUser/{userId}")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateApplicationUser(Guid userId, [FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        var result = await Mediator.Send(new UpdateUserCommand() { UserId = userId, UpdateUserRequestDto = updateUserRequestDto });

        return Ok(result);
    }


    // Delete User
    [HttpDelete("deleteuser/{userId}")]
    public async Task<ActionResult<DeleteUserResponse>> DeleteApplicationUser(Guid userId)
    {
        var result = await Mediator.Send(new DeleteUserCommand() { UserId = userId });

        return Ok(result);
    }



}
