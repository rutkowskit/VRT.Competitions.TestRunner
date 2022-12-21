using VRT.Competitions.TestRunner.Application.TestTasks;
using VRT.Competitions.TestRunner.Application.TestTasks.States;

namespace VRT.Competitions.TestRunner.Cli;
public sealed partial class TestTaskWithState : ITestTaskWithState
{
    public TestTaskWithState(TestTaskParams testParams)
    {
        Title = Path.GetFileName(testParams.InputFilePath); ;
        TestParams = testParams;
    }
    public BaseTestTaskState? State { get; set; }

    public string Title { get; }
    public TestTaskParams TestParams { get; }

    public void EnterState(BaseTestTaskState state)
    {
        State = state;

        if(state is CompletedTestTaskState || state is ErrorTestTaskState)
        {
            Console.Write($"{Title} | {state.RunTime} | {(state.IsOk ? "OK" : "NotOk")} | {state.Message}");
        }
    }
}
