using System.Text.RegularExpressions;
using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Application.TestTasks.States;

public sealed partial class CompletedTestTaskState : BaseTestTaskState
{
    public CompletedTestTaskState(ITestTaskContext context,
        ShellCommandOutput shellOutput) : base(context)
    {
        RunTime = shellOutput.RunTime;
        var hasExpectedOutput = HasExpectedOutput(shellOutput);
        IsOk = hasExpectedOutput;
        Message = hasExpectedOutput ? "" : "Output does not match";
        RunTime = shellOutput.RunTime;
    }
    public override string Name => "Completed";

    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        TransitionTo(new ReadyToRunTestTaskState(Context));
        return Task.CompletedTask;
    }
    private bool HasExpectedOutput(ShellCommandOutput shellOutput)
    {
        var expectedOut = File.ReadAllText(Context.TestParams.OutputFilePath);
        var str1 = MatchWhiteSpaces().Replace(shellOutput.Output ?? "", string.Empty);
        var str2 = MatchWhiteSpaces().Replace(expectedOut ?? "", string.Empty);
        var isOk = string.Compare(str1, str2, StringComparison.InvariantCulture) == 0;
        return isOk;
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex MatchWhiteSpaces();
}
