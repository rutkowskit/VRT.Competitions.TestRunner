using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using System.IO;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Wpf.TestTasks;

namespace VRT.Competitions.TestRunner.Wpf;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly IStateService _stateService;
    private readonly IShellService _shellService;

    private record MainWindowViewModelState(string? ExecutableFilePath, string? TestsDirectoryPath);

    public MainWindowViewModel(IStateService stateService, IShellService shellService)
    {
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
    private TestTaskViewModel? _currentTestTask;

    [ObservableProperty]
    private int _progress;

    [ObservableProperty]
    private string? _progressText;


    [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanStartAsync), IncludeCancelCommand = true)]
    private async Task StartAsync(CancellationToken cancellationToken)
    {
        TestTasks = Array.Empty<TestTaskViewModel>();
        Progress = 0;
        TestTasks = GetTestTasks();
        int cnt = 0;
        int total = TestTasks.Count;
        await SaveState();
        foreach (var t in TestTasks)
        {
            CurrentTestTask = t;
            if (cancellationToken.IsCancellationRequested) break;
            await t.StartAsync(cancellationToken);
            cnt++;
            UpdateProgress(cnt, total);
            
        }
    }
    private void UpdateProgress(int cnt, int total)
    {
        if (cnt % 4 == 0)
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

    private IReadOnlyCollection<TestTaskViewModel> GetTestTasks()
    {
        var testDir = TestsDirectoryPath!;
        var inFiles = Directory.GetFiles(testDir, "*.in", SearchOption.AllDirectories);
        var outFiles = Directory.GetFiles(testDir, "*.out", SearchOption.AllDirectories);

        var result = inFiles
            .Join(outFiles, ToFileNameKey, ToFileNameKey, (inFile, outFile) => (inFile, outFile))
            .Where(t => string.IsNullOrEmpty(t.inFile) is false)
            .Distinct()
            .Select(CreateTestTaskViewModelParams)
            .Select(t => new TestTaskViewModel(t))
            .OrderBy(o => o.Priority)
            .ThenBy(o => o.Title)
            .ToArray();

        return result;
    }

    private TestTaskViewModelParams CreateTestTaskViewModelParams((string inFile, string outFile) files)
    {
        return new TestTaskViewModelParams()
        {
            ExecutableFilePath = ExecutableFilePath!,
            InputFilePath = files.inFile,
            OutputFilePath = files.outFile,
            Shell = _shellService
        };
    }

    private string ToFileNameKey(string fullFilePath) => Path.GetFileNameWithoutExtension(fullFilePath);

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
