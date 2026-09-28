using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Implementations;
using UserManagement.Services.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
            .AddScoped<UserService>()
            .AddScoped<ILogService, LogService>()
            .AddScoped<IUserService>(sp => new UserServiceLogger(
                sp.GetRequiredService<UserService>(),
                sp.GetRequiredService<ILogService>(),
                sp.GetRequiredService<ICurrentUserAccessor>()));
}
