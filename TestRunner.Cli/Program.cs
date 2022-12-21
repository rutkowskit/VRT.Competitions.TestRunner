using Microsoft.Extensions.DependencyInjection;
using VRT.Competitions.TestRunner.Cli;
using VRT.Competitions.TestRunner.Infrastructure;

if (args.Length < 2)
{
    Console.Error.WriteLine("Application requires 2 parameters.");
    Console.Error.WriteLine("TestRunnerCli <exeFilePath> <testFilesDirectoryPath>");
    return -1;
}

var services = new ServiceCollection()
    .AddInfrastructure()
    .AddPresentation()
    .BuildServiceProvider();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

var viewModel = services.GetRequiredService<ConsoleAppViewModel>();
await viewModel.StartAsync(args[0], args[1], cts.Token);
return 0;