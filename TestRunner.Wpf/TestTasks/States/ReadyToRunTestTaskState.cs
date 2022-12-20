using Microsoft.Extensions.DependencyInjection;
using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Wpf.TestTasks.States;
public sealed class ReadyToRunTestTaskState : BaseTestTaskState
{
    public ReadyToRunTestTaskState(ITestTaskContext context) : base(context)
    {
    }
    public override string Name => "Ready";
    public override bool CanStart => true;

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cmdParams = new ShellCommandParams(Context.TestParams.ExecutableFilePath)
        {
            StdInFilePath = Context.TestParams.InputFilePath
        };

        var testingTask = Context.TestParams.Shell
            .RunShellCommandAsync(cmdParams, cancellationToken);

        var newState = new RunningTestTaskState(Context, testingTask);
        Context.EnterState(newState);
        await newState.StartAsync(cancellationToken);
    }
}
