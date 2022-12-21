using CSharpFunctionalExtensions;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Application.TestTasks;

namespace VRT.Competitions.TestRunner.Application.TestTasks.States;

public sealed class RunningTestTaskState : BaseTestTaskState
{
    private readonly Task<Result<ShellCommandOutput>> _testingTask;

    public RunningTestTaskState(ITestTaskContext context,
        Task<Result<ShellCommandOutput>> testingTask) : base(context)
    {
        _testingTask = Result.Success()
            .BindTry(() => testingTask)
            .Tap(TransitionToCompletedState)
            .TapError(TransitionToErrorState);
    }
    public override string Name => "Running";

    public override async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _testingTask;
    }
    private void TransitionToCompletedState(ShellCommandOutput result)
    {
        TransitionTo(new CompletedTestTaskState(Context, result));
    }
}
