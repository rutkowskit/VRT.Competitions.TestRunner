namespace VRT.Competitions.TestRunner.Application.TestTasks.States;

public sealed class ErrorTestTaskState : BaseTestTaskState
{
    public ErrorTestTaskState(ITestTaskContext context, string errorMessage) : base(context)
    {
        Message = errorMessage;        
    }
    public override string Name => "Error";

    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        TransitionTo(new ReadyToRunTestTaskState(Context));
        return Task.CompletedTask;
    }
}
