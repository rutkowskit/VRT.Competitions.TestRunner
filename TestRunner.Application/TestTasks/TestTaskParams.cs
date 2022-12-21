namespace VRT.Competitions.TestRunner.Application.TestTasks;

public sealed class TestTaskParams
{
    required public string ExecutableFilePath { get; init; }
    required public string InputFilePath { get; init; }
    required public string OutputFilePath { get; init; }
    public int Priority { get; init; }
}
