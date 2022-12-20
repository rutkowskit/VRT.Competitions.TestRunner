using Microsoft.Extensions.DependencyInjection;

namespace VRT.Competitions.TestRunner.Wpf;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        return services;
    }
}
