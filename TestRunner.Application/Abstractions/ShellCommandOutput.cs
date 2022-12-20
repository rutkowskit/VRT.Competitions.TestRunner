namespace VRT.Competitions.TestRunner.Application.Abstractions;

public record ShellCommandOutput(int ExitCode, string Output, TimeSpan RunTime);
