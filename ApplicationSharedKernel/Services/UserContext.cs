using ApplicationSharedKernel.HelperClasses;
using ApplicationSharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Security.Claims;

namespace SharedKernel.Application.Services;


public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;
        if (user == null)
        {
            throw new InvalidOperationException("User context is not present");
        }

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var userName = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var firstName = user.FindFirst(c => c.Type == ClaimTypes.GivenName)!.Value;
        var lastName = user.FindFirst(c => c.Type == ClaimTypes.Surname)!.Value;
        var gender = user.FindFirst(c => c.Type == ClaimTypes.Gender)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
        var nationality = user.FindFirst(c => c.Type == ClaimTypes.Country)!.Value;

        var dateOfBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var dateOfBirth = dateOfBirthString == null     // if this is null,
            ? (DateOnly?)null                           // assign a null value to our DateOnly variable but if it is not a null value,
            : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");     // Parse it as a string value in this specified format 


        var createdAtString = user.FindFirst(c => c.Type == "CreatedAt")?.Value;
        var createdAt = createdAtString == null     // if this is null,
            ? (DateTime?)null                           // assign a null value to our DateOnly variable but if it is not a null value,
            : DateTime.ParseExact(createdAtString, new string[] { "MM.dd.yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None);     // Parse it as a string value in this specified format 

        var lastLoginString = user.FindFirst(c => c.Type == "LastLogin")?.Value;
        var lastLogin = lastLoginString == null     // if this is null,
            ? (DateTime?)null                           // assign a null value to our DateOnly variable but if it is not a null value,
            : DateTime.ParseExact(lastLoginString, new string[] { "MM.dd.yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None);     // Parse it as a string value in this specified format 


        return new CurrentUser(userId, userName, email, firstName, lastName, gender, roles, nationality, dateOfBirth, createdAt, lastLogin);
    }
}
