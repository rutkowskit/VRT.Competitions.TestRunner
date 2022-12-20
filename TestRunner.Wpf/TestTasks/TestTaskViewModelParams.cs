using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Wpf.TestTasks;

public sealed class TestTaskViewModelParams
{
    required public string ExecutableFilePath { get; init; }
    required public string InputFilePath { get; init; }
    required public string OutputFilePath { get; init; }
    required public IShellService Shell { get; init; }
}
