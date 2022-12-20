using CliWrap;
using CliWrap.Buffered;
using CSharpFunctionalExtensions;
using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Infrastructure.Services;

public sealed class ShellService : IShellService
{
    public async Task<Result<ShellCommandOutput>> RunShellCommandAsync(ShellCommandParams parameters,
        CancellationToken cancellationToken = default)
    {
        using var linkedCancellationToken = CreateTokenSourceWithTimeout(parameters.TimeOutInMilliseconds, cancellationToken);

        var result = await CreateCliCommand(parameters)
            .Bind(cmd => ExecuteCliCommand(cmd, linkedCancellationToken.Token))
            .Bind(HandleCliResult);

        return result;
    }
    private static Result<ShellCommandOutput> HandleCliResult(BufferedCommandResult cliResult)
    {
        if (string.IsNullOrWhiteSpace(cliResult.StandardError) is false)
        {
            return Result.Failure<ShellCommandOutput>(cliResult.StandardError);
        }
        return new ShellCommandOutput(cliResult.ExitCode, cliResult.StandardOutput, cliResult.RunTime);
    }
    private static async Task<Result<BufferedCommandResult>> ExecuteCliCommand(Command command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await command.ExecuteBufferedAsync(cancellationToken);
            return result;
        }
        catch (OperationCanceledException)
        {
            return Result.Failure<BufferedCommandResult>("Operation canceled");
        }
        catch (Exception ex)
        {
            return Result.Failure<BufferedCommandResult>(ex.ToString());
        }
    }
    private static Result<Command> CreateCliCommand(ShellCommandParams parameters)
    {
        if (File.Exists(parameters.ExecutableFile) is false)
        {
            return Result.Failure<Command>("Executable file does not exist");
        }
        if (parameters.StdInFilePath is not null && File.Exists(parameters.StdInFilePath) is false)
        {
            return Result.Failure<Command>("Input data file does not exist");
        }
        var result = Cli.Wrap(parameters.ExecutableFile);
        if (parameters.StdInFilePath is not null)
        {
            result = result.WithStandardInputPipe(PipeSource.FromFile(parameters.StdInFilePath!));
        }
        return result;
    }
    private static CancellationTokenSource CreateTokenSourceWithTimeout(int timeOutInMilliseconds,
        CancellationToken callerCancellationToken)
    {
        var linkedCancellationToken = CancellationTokenSource
            .CreateLinkedTokenSource(callerCancellationToken);
        var timeOut = timeOutInMilliseconds > 0
            ? TimeSpan.FromMilliseconds(timeOutInMilliseconds)
            : TimeSpan.FromSeconds(20);
        linkedCancellationToken.CancelAfter(timeOut);
        return linkedCancellationToken;
    }
}
