namespace VRT.Competitions.TestRunner.Application.TestTasks.States;
public abstract class BaseTestTaskState
{    
    protected BaseTestTaskState(ITestTaskContext context)
    {
        Context = context;
    }
    public abstract string Name { get; }
    public TimeSpan? RunTime { get; protected set; }
    public string? Message { get; protected set; }
    public bool IsOk { get; protected set; }
    protected ITestTaskContext Context { get; }
    public abstract Task StartAsync(CancellationToken cancellationToken = default);

    protected void TransitionToErrorState(string errorMessage)
    {
        var errorState = new ErrorTestTaskState(Context, errorMessage);
        TransitionTo(errorState);
    }
    protected void TransitionTo(BaseTestTaskState newState)
    {
        Context.EnterState(newState);
    }
}
