using CSharpFunctionalExtensions;

namespace VRT.Competitions.TestRunner.Application.Abstractions;
public interface IShellService
{
    Task<Result<ShellCommandOutput>> RunShellCommandAsync(ShellCommandParams parameters, CancellationToken cancellationToken = default);
}
