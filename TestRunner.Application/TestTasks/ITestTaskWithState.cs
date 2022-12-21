using VRT.Competitions.TestRunner.Application.TestTasks.States;

namespace VRT.Competitions.TestRunner.Application.TestTasks
{
    public interface ITestTaskWithState
    {
        TestTaskParams TestParams { get; }
        void EnterState(BaseTestTaskState state);
    }
}
