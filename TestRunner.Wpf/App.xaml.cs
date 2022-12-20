
using Microsoft.Extensions.DependencyInjection;
using VRT.Competitions.TestRunner.Infrastructure;

namespace VRT.Competitions.TestRunner.Wpf;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    public App()
    {
        Services ??= new ServiceCollection()
            .AddInfrastructure()
            .AddPresentation()
            .BuildServiceProvider();
    }
}
