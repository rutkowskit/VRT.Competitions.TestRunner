namespace VRT.Competitions.TestRunner.Application.TestTasks.Queries.GetTestTasks;

public sealed class GetTestTasksDto
{
    required public string ExecutableFilePath { get; init; }
    required public string InputFilePath { get; init; }
    required public string OutputFilePath { get; init; }
}
