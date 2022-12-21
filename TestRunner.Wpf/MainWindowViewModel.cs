using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using System.IO;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Application.TestTasks;
using VRT.Competitions.TestRunner.Application.TestTasks.Commands.RunTestTasks;
using VRT.Competitions.TestRunner.Application.TestTasks.Queries.GetTestTasks;
using VRT.Competitions.TestRunner.Wpf.TestTasks;

namespace VRT.Competitions.TestRunner.Wpf;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly IStateService _stateService;
    private readonly IShellService _shellService;
    private readonly IMediator _mediator;

    private record MainWindowViewModelState(string? ExecutableFilePath, string? TestsDirectoryPath);

    public MainWindowViewModel(IStateService stateService, IShellService shellService,
        IMediator mediator)
    {
        _mediator = mediator;
        _stateService = stateService;
        _shellService = shellService;
        _ = LoadState();
    }
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private string? _executableFilePath;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    private string? _testsDirectoryPath;

    [ObservableProperty]
    private IReadOnlyCollection<TestTaskViewModel>? _testTasks;

    [ObservableProperty]
    private ITestTaskWithState? _currentTestTask;

    [ObservableProperty]
    private int _progress;

    [ObservableProperty]
    private string? _progressText;


    [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanStartAsync), IncludeCancelCommand = true)]
    private async Task StartAsync(CancellationToken cancellationToken)
    {
        TestTasks = Array.Empty<TestTaskViewModel>();
        Progress = 0;
        await SaveState();
        TestTasks = await GetTestTasks(cancellationToken);
        var command = new RunTestTasksCommand(TestTasks)
        { 
            OnProgressCallback = UpdateProgress,
            OnBeforeRunTestCallback = t => CurrentTestTask = t,
        };
        await _mediator.Send(command, cancellationToken);
    }
    private void UpdateProgress((int cnt, int total) progress)
    {
        var (cnt,total) = progress;
        if (cnt % 4 == 0 || cnt == total)
        {
            Progress = Math.Min((int)(100f * cnt / total), 100);
            ProgressText = $"{cnt:######} of {total:######}";
        }
    }
    private bool CanStartAsync()
    {
        return string.IsNullOrEmpty(ExecutableFilePath) is false
            && File.Exists(ExecutableFilePath)
            && string.IsNullOrEmpty(TestsDirectoryPath) is false
            && Directory.Exists(TestsDirectoryPath);
    }

    private async Task<IReadOnlyCollection<TestTaskViewModel>> GetTestTasks(CancellationToken cancellationToken)
    {
        var query = new GetTestTasksQuery(ExecutableFilePath!, TestsDirectoryPath!);
        var result = await _mediator.Send(query, cancellationToken)
            .Map(ToTestTaskViewModel)
            .Compensate(_ => Array.Empty<TestTaskViewModel>());
        return result.Value;        
    }
    private IReadOnlyCollection<TestTaskViewModel> ToTestTaskViewModel(IEnumerable<TestTaskParams> testParams)
    {
        var result = testParams
            .Select(t => new TestTaskViewModel(t))
            .OrderBy(o => o.TestParams.Priority)
            .ThenBy(o => o.Title)
            .ToArray();
        return result;
    }

    private async Task SaveState()
    {
        _ = await _stateService.SaveStateAsync(new MainWindowViewModelState(ExecutableFilePath, TestsDirectoryPath));
    }
    private async Task LoadState()
    {
        await _stateService.LoadStateAsync<MainWindowViewModelState>()
            .Tap(state => ExecutableFilePath = state.ExecutableFilePath)
            .Tap(state => TestsDirectoryPath = state.TestsDirectoryPath);
    }
}
