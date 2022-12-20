using VRT.Competitions.TestRunner.Wpf.TestTasks.States;

namespace VRT.Competitions.TestRunner.Wpf.TestTasks;
public interface ITestTaskContext
{
    TestTaskViewModelParams TestParams { get; }
    TimeSpan? RunTime { set; }
    string? Message { set; }
    bool IsOk { set; }
    void EnterState(BaseTestTaskState state);
}
