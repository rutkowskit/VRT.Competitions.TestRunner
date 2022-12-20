using Microsoft.Extensions.DependencyInjection;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Infrastructure.Services;

namespace VRT.Competitions.TestRunner.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IShellService, ShellService>()
            .AddSingleton<IStateService, StateService>();
        return services;
    }
}
