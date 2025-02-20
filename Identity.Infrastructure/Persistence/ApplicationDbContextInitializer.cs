using Identity.Domain.Entities;
using Identity.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Identity.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _applicationDbContext; 

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext applicationDbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }




    public async Task SeedIdentityData()
    {
        // Add roles before adding users... since users make use of roles
        if (!_roleManager.Roles.Any())
        {
            var applicationRoles = new ApplicationRole[]
            {
                    new ApplicationRole
                    {
                        Name = "GodsEye",
                        NormalizedName = "GODSEYE",
                        Description = "The highest position and one of priviledge in the organization who alone has the authority to assign roles to other users and can perform any duty/access any endpoint/resource whatsoever within this organization"
                    },
                    new ApplicationRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        Description = "An Executive position with responsibilities but can neither assign/remove users from roles"
                    },
                    new ApplicationRole
                    {
                        Name = "Staff",
                        NormalizedName = "STAFF",
                        Description = "The default position for all employee within our organization"
                    },
                    new ApplicationRole
                    {
                        Name = "StandardUser",
                        NormalizedName = "STANDARDUSER",
                        Description = "The default position for everyone/visitors who logs-in/authenticate in our app"
                    }

            };

            var adminRoleClaims = new Claim[]
            {
                    new Claim(ClaimTypes.AuthorizationDecision, "edit.post"),
                    new Claim(ClaimTypes.AuthorizationDecision, "delete.post"),
                    new Claim(ClaimTypes.AuthorizationDecision, "create.post"),
                    new Claim(ClaimTypes.AuthorizationDecision, "view.post"),
                //new Claim(ClaimTypes.AuthorizationDecision, "create.comment")
            };

            var standardUserRoleClaims = new Claim[]
            {
                    new Claim(ClaimTypes.AuthorizationDecision, "create.comment"),
                    new Claim(ClaimTypes.AuthorizationDecision, "edit.comment"),
                    new Claim(ClaimTypes.AuthorizationDecision, "view.comment"),
                    new Claim(ClaimTypes.AuthorizationDecision, "delete.comment")
                //new Claim(CustomClaimTypes.Permission, "projects.update")
            };

            var godsEyeRoleClaims = new Claim[]
            {
                    new Claim(ClaimTypes.AuthorizationDecision, "assign.user.role"),
                    new Claim(ClaimTypes.AuthorizationDecision, "unassign.user.role"),
                    new Claim(ClaimTypes.AuthorizationDecision, "delete.user"),
                    new Claim(ClaimTypes.AuthorizationDecision, "update.user")
            };


            // create roles
            foreach (var role in applicationRoles!)
            {
                await _roleManager.CreateAsync(role);
            }

            // adding role claims - admin
            foreach (var claim in adminRoleClaims)
            {
                var adminRole = await _roleManager.FindByNameAsync(AppUserRoles.Admin);
                await _roleManager.AddClaimAsync(adminRole, claim);
            }
            // standard role
            foreach (var claim in standardUserRoleClaims)
            {
                var standardUserRole = await _roleManager.FindByNameAsync(AppUserRoles.StandardUser);
                await _roleManager.AddClaimAsync(standardUserRole, claim);
            }
            // godseye role
            foreach (var claim in godsEyeRoleClaims)
            {
                var godsEyeRole = await _roleManager.FindByNameAsync(AppUserRoles.GodsEye);
                await _roleManager.AddClaimAsync(godsEyeRole, claim);
            }

            _applicationDbContext.SaveChanges();

        }


        if (!_userManager.Users.Any())
        {
            
            var applicationUsers = new ApplicationUser[]
            {
                    new ApplicationUser
                    {
                        FirstName = "Gods",
                        LastName = "Eye",
                        Email = "godsEye@gmail.com",
                        NormalizedEmail = "GODSEYE@GMAIL.COM",
                        ConstUserName = "godsEyeNumber1",
                        UserName = "godsEyeNumber1",
                        Gender = "Male",
                        PhoneNumber = "08109713734",
                        PhoneNumberConfirmed = true,
                        DateOfBirth = new DateOnly(1990, 01, 01),
                        Nationality = "Nigeria",
                        EmailConfirmed = true,
                        TwoFactorEnabled = true
                    },
                    new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "adminuser@gmail.com",
                        NormalizedEmail = "ADMINUSER@GMAIL.COM",
                        ConstUserName = "adminNumber2",
                        UserName = "adminNumber2",
                        Gender = "Male",
                        PhoneNumber = "08155014157",
                        PhoneNumberConfirmed = true,
                        DateOfBirth = new DateOnly(1991, 02, 15),
                        Nationality = "Nigeria",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false
                    },
                    new ApplicationUser
                    {
                        FirstName = "Staff",
                        LastName = "User",
                        Email = "staffuser@gmail.com",
                        NormalizedEmail = "STAFFUSER@GMAIL.COM",
                        ConstUserName = "staffUserNumber3",
                        UserName = "staffUserNumber3",
                        Gender = "Female",
                        PhoneNumber = "08109713734",
                        PhoneNumberConfirmed = true,
                        DateOfBirth = new DateOnly(1992, 10, 18),
                        Nationality = "Nigeria",
                        EmailConfirmed = true,
                        TwoFactorEnabled = true
                    },
                    new ApplicationUser
                    {
                        FirstName = "Standard",
                        LastName = "User",
                        Email = "standarduser@gmail.com",
                        NormalizedEmail = "STANDARDUSER@GMAIL.COM",
                        ConstUserName = "standardUserNumber4",
                        UserName = "standardUserNumber4",
                        Gender = "Female",
                        PhoneNumber = "08155014157",
                        PhoneNumberConfirmed = true,
                        DateOfBirth = new DateOnly(1993, 08, 24),
                        Nationality = "Nigeria",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false
                    }

            };

            // create users
            foreach (var user in applicationUsers!)
            {
                await _userManager.CreateAsync(user, "Pa$$word4User");
            }
            // add all users to default role
            foreach (var user in applicationUsers!)
            {
                await _userManager.AddToRoleAsync(user, "StandardUser");
            }
            // add users to special roles
            foreach (var user in applicationUsers!)
            {
                if (user.Email == "adminuser@gmail.com") await _userManager.AddToRoleAsync(user, "Admin");
                if (user.Email == "godsEye@gmail.com") await _userManager.AddToRoleAsync(user, "GodsEye");
            }


            _applicationDbContext.SaveChanges();
        }

        if (_applicationDbContext.ChangeTracker.HasChanges()) await _applicationDbContext.SaveChangesAsync();

    }

}
