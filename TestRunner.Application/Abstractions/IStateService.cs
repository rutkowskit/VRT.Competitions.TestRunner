using CSharpFunctionalExtensions;

namespace VRT.Competitions.TestRunner.Application.Abstractions;
public interface IStateService
{
    /// <summary>
    /// Loads the state of the given type
    /// </summary>
    /// <typeparam name="TState">State type parameter</typeparam>
    /// <param name="name">Optional name of the previously saved state. If not specified, the name of the TState type will be used instead</param>
    /// <returns>Loaded state</returns>
    Task<Result<TState>> LoadStateAsync<TState>(string? name = null);

    /// <summary>
    /// Saves the given state
    /// </summary>
    /// <typeparam name="TState">Type of the state to save</typeparam>
    /// <param name="state">State to save</param>
    /// <param name="name">Optional name for the state to save. If unspecified, the name of the TState will be used instead</param>
    /// <returns></returns>
    Task<Result> SaveStateAsync<TState>(TState state, string? name = null)
        where TState : notnull;
}
