namespace VRT.Competitions.TestRunner.Wpf.TestTasks.States;
public abstract class BaseTestTaskState
{
    public abstract string Name { get; }
    public virtual bool IsError { get; }
    public virtual string? ErrorMessage { get; }
    public virtual bool CanStart { get; }
    protected BaseTestTaskState(ITestTaskContext context)
    {
        Context = context;        
    }
    protected ITestTaskContext Context { get; }
    public abstract Task StartAsync(CancellationToken cancellationToken=default);

    protected void TransitionToErrorState(string errorMessage)
    {
        var errorState = new ErrorTestTaskState(Context,errorMessage);
        TransitionTo(errorState);
    }
    protected void TransitionTo(BaseTestTaskState newState)
    {        
        Context.EnterState(newState);
    }
}
