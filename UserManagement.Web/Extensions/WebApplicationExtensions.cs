using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data.Entities;

namespace UserManagement.Web.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SetupDemoAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        foreach (var role in new[] { "Admin", "User" })
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var roleResult = await roleManager.CreateAsync(new ApplicationRole(role));

            if (!roleResult.Succeeded)
            {
                throw new Exception($"Failed to create role {role}");
            }
        }

        await CreateUser(userManager, new ApplicationUser
            {
                UserName = "user",
                Email = "user@example.com",
                EmailConfirmed = true
            },
            password: "dem0useR!",
            roles: ["User"]);

        await CreateUser(userManager, new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                },
                password: "dem0admiN!",
                roles: ["Admin", "User"]);
    }

    private static async Task CreateUser(UserManager<ApplicationUser> userManager, ApplicationUser user, String password, IEnumerable<String> roles)
    {
        var userResult = await userManager.CreateAsync(user, password);
        if (!userResult.Succeeded)
        {
            throw new Exception($"Failed to create user {user.UserName}");
        }

        foreach (String role in roles)
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Failed to add user {user.UserName} to role {role}");
            }
        }
    }
}
