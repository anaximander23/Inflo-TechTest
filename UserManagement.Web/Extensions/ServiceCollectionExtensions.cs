using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using UserManagement.Data.Entities;

namespace UserManagement.Web.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDemoAuth(this IServiceCollection services)
    {
        services
            .AddDbContext<AuthContext>()
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AuthContext>();

        return services;
    }
}
