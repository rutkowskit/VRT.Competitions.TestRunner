using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Application.TestTasks.States;

namespace VRT.Competitions.TestRunner.Application.TestTasks.Commands.RunTestTasks;
internal sealed class TestTaskRunContext : ITestTaskContext
{
    private readonly ITestTaskWithState _testTask;

    public TestTaskRunContext(ITestTaskWithState testTask)
    {
        _testTask = testTask;
        EnterState(new ReadyToRunTestTaskState(this));        
    }
    public ITestTaskWithState DecoratedTestTask => _testTask;
    public TestTaskParams TestParams => _testTask.TestParams;
    required public IShellService Shell { get; init; }    
    public BaseTestTaskState? State { get; private set; }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return State!.StartAsync(cancellationToken);
    }
    public void EnterState(BaseTestTaskState state)
    {
        _testTask.EnterState(state);
        State = state;
    }
}
