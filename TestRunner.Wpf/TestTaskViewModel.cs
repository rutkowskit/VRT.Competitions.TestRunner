using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using VRT.Competitions.TestRunner.Application.TestTasks;
using VRT.Competitions.TestRunner.Application.TestTasks.States;
using VRT.Competitions.TestRunner.Wpf.Common;

namespace VRT.Competitions.TestRunner.Wpf.TestTasks;
public sealed partial class TestTaskViewModel : BaseViewModel, ITestTaskWithState
{
    public TestTaskViewModel(TestTaskParams testParams)
    {
        Title = Path.GetFileName(testParams.InputFilePath);
        TestParams = testParams;
    }

    [ObservableProperty]
    private BaseTestTaskState? _state;

    public string Title { get; }
    public TestTaskParams TestParams { get; }

    public void EnterState(BaseTestTaskState state)
    {
        State = state;
    }
}
