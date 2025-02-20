
// ACCOUNT CONTROLLER
global using Identity.Application.Features.UsersEndpoints.AuthenticateUser;
global using Identity.Application.Features.UsersEndpoints.ChangeEmailConfirmation;
global using Identity.Application.Features.UsersEndpoints.ChangeEmailRequest;
global using Identity.Application.Features.UsersEndpoints.ChangePassword;
global using Identity.Application.Features.UsersEndpoints.DisableOrEnableTwoFacAuth;
global using Identity.Application.Features.UsersEndpoints.EmailConfirmation;
global using Identity.Application.Features.UsersEndpoints.ForgotPassword;
global using Identity.Application.Features.UsersEndpoints.GetMyDetails;
global using Identity.Application.Features.UsersEndpoints.RefreshToken;
global using Identity.Application.Features.UsersEndpoints.RegisterUser;
global using Identity.Application.Features.UsersEndpoints.ResendEmailConfirmation;
global using Identity.Application.Features.UsersEndpoints.ResetPassword;
global using Identity.Application.Features.UsersEndpoints.UpdateUserDetails;
global using Identity.Application.Features.UsersEndpoints.VerifyTwoFacAuth;
global using Identity.Shared.DTO;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using SharedKernel.Api.Controllers;
global using System.Security.Claims;
global using Asp.Versioning;



// MANAGE_ACCOUNT_CONTROLLER
global using ApplicationSharedKernel.HelperClasses;
global using Identity.Application.Features.UserManagementEndpoints.Commands.AddUserClaim;
global using Identity.Application.Features.UserManagementEndpoints.Commands.AssignUserRole;
global using Identity.Application.Features.UserManagementEndpoints.Commands.DeleteAllClaimsForAUser;
global using Identity.Application.Features.UserManagementEndpoints.Commands.DeleteUser;
global using Identity.Application.Features.UserManagementEndpoints.Commands.LockOutUser;
global using Identity.Application.Features.UserManagementEndpoints.Commands.RemoveUserClaim;
global using Identity.Application.Features.UserManagementEndpoints.Commands.RevokeRefreshToken;
global using Identity.Application.Features.UserManagementEndpoints.Commands.UnAssignUserRole;
global using Identity.Application.Features.UserManagementEndpoints.Commands.UnlockUser;
global using Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUser;
global using Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUserClaim;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;
global using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;
global using SharedKernel.Domain.HelperClasses;

// MANAGE_ROLE_CONTROLLER
global using Identity.Application.Features.RoleManagement.Commands.AddApplicationRole;
global using Identity.Application.Features.RoleManagement.Commands.AddRoleClaim;
global using Identity.Application.Features.RoleManagement.Commands.DeleteApplicationRole;
global using Identity.Application.Features.RoleManagement.Commands.RemoveRoleClaim;
global using Identity.Application.Features.RoleManagement.Commands.UpdateApplicationRole;
global using Identity.Application.Features.RoleManagement.Queries.GetAllApplicationRoles;
global using Identity.Application.Features.RoleManagement.Queries.GetAllClaimsForARole;
global using Identity.Application.Features.RoleManagement.Queries.GetRoleById;
global using Identity.Application.Features.RoleManagement.Queries.GetRoleByName;