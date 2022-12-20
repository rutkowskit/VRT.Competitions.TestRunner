using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using VRT.Competitions.TestRunner.Wpf.Common;
using VRT.Competitions.TestRunner.Wpf.TestTasks.States;

namespace VRT.Competitions.TestRunner.Wpf.TestTasks;
public sealed partial class TestTaskViewModel : BaseViewModel, ITestTaskContext
{
    public TestTaskViewModel(TestTaskViewModelParams testParams)
    {
        var title = Path.GetFileName(testParams.InputFilePath);
        Title = title;
        TestParams = testParams;
        Priority = TryGetPriority(title, out var priority)
            ? priority
            : 0;
        EnterState(new ReadyToRunTestTaskState(this));
    }

    [ObservableProperty]
    private BaseTestTaskState? _state;

    [ObservableProperty]
    private TimeSpan? _runTime;

    [ObservableProperty]
    private string? _message;

    [ObservableProperty]
    private bool _isOk;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return State!.StartAsync(cancellationToken);
    }

    public string Title { get; }
    public int Priority { get; }
    public TestTaskViewModelParams TestParams { get; }

    public void EnterState(BaseTestTaskState state)
    {
        State = state;
    }

    private static bool TryGetPriority(string inputFilePath, out int priority)
    {
        var numberMatch = MatchNumber().Match(inputFilePath);
        priority = numberMatch.Success
            ? int.Parse(numberMatch.Value)
            : 0;
        return numberMatch.Success;
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex MatchNumber();
}
