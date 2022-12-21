using Microsoft.Extensions.DependencyInjection;

namespace VRT.Competitions.TestRunner.Cli;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddTransient<ConsoleAppViewModel>();
        return services;
    }
}
