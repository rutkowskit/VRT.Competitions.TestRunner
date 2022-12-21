using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Application.TestTasks.States;
public sealed class ReadyToRunTestTaskState : BaseTestTaskState
{
    public ReadyToRunTestTaskState(ITestTaskContext context) : base(context)
    {
    }
    public override string Name => "Ready";
    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cmdParams = new ShellCommandParams(Context.TestParams.ExecutableFilePath)
        {
            StdInFilePath = Context.TestParams.InputFilePath
        };

        var testingTask = Context.Shell
            .RunShellCommandAsync(cmdParams, cancellationToken);

        var newState = new RunningTestTaskState(Context, testingTask);
        Context.EnterState(newState);
        await newState.StartAsync(cancellationToken);
    }
}
