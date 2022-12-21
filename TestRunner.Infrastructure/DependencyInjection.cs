using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VRT.Competitions.TestRunner.Application;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Infrastructure.Services;

namespace VRT.Competitions.TestRunner.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IShellService, ShellService>()
            .AddSingleton<IStateService, StateService>()
            .AddSingleton<IDirectoryService, DirectoryService>()
            .AddMediatR(GetMediatrAssemblies().ToArray());
        return services;
    }

    private static IEnumerable<Assembly> GetMediatrAssemblies()
    {
        yield return typeof(DependencyInjection).Assembly;
        yield return typeof(IApplicationAssemblyMarker).Assembly;
    }
}
