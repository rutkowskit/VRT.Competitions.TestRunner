namespace VRT.Competitions.TestRunner.Application.Abstractions;

public sealed record ShellCommandParams(string ExecutableFile)
{
    /// <summary>
    /// The path to the file whose content is to be redirected to standard input
    /// </summary>
    public string? StdInFilePath { get; init; }    
    public int TimeOutInMilliseconds { get; init; }
}
