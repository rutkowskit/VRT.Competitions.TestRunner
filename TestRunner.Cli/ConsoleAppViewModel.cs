using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using VRT.Competitions.TestRunner.Application.TestTasks.Commands.RunTestTasks;
using VRT.Competitions.TestRunner.Application.TestTasks.Queries.GetTestTasks;

namespace VRT.Competitions.TestRunner.Cli;

public sealed partial class ConsoleAppViewModel
{
    private readonly ISender _mediator;

    public ConsoleAppViewModel(ISender mediator)
    {
        _mediator = mediator;
    }
    public async Task StartAsync(
        string executableFilePath, 
        string testDirectoryPath,
        CancellationToken cancellationToken = default)
    {
        await Result
            .Success(new GetTestTasksQuery(executableFilePath, testDirectoryPath))
            .BindTry(query => _mediator.Send(query, cancellationToken))
            .Map(testParams => testParams.Select(tp => new TestTaskWithState(tp)).ToArray())
            .Map(testTasks => new RunTestTasksCommand(testTasks) { OnProgressCallback = OnProgress })
            .BindTry(command => _mediator.Send(command, cancellationToken))
            .TapError(err => Console.Error.WriteLine(err));
    }
    private void OnProgress((int cnt, int total) progress)
    {
        var (cnt, total) = progress;
        var totalStr = total.ToString();
        var format = new string('0',totalStr.Length);
        var percent = total > 0 ? Math.Min(100,100 * cnt / total) : 0;
        Console.WriteLine($"({cnt.ToString(format)} of {total} - {percent}%)");
    }
}
