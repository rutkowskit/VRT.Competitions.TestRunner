using CSharpFunctionalExtensions;
using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Infrastructure.Services;
public sealed class StateService : IStateService
{
    private readonly string _statesDirectory;
    public StateService()
    {
        _statesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TestRunner", "States");
        Directory.CreateDirectory(_statesDirectory);
    }
    public async Task<Result<TState>> LoadStateAsync<TState>(string? name = null)
    {
        await Task.Yield();

        var stateName = name ?? typeof(TState).Name;
        var filePath = GetStateFilePath<TState>(stateName);
        if (File.Exists(filePath) is false)
        {
            return Result.Failure<TState>($"The State with name: {stateName} not found");
        }
        try
        {
            var json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            var result = System.Text.Json.JsonSerializer.Deserialize<TState>(json);
            return result ?? Result.Failure<TState>($"The State with name:  {stateName} is empty");
        }
        catch (Exception ex)
        {
            return Result.Failure<TState>(ex.Message);
        }
    }

    public async Task<Result> SaveStateAsync<TState>(TState state, string? name = null)
        where TState : notnull
    {
        await Task.Yield();

        var filePath = GetStateFilePath<TState>(name);
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    private string GetStateFilePath<TState>(string? name)
    {
        var stateName = string.IsNullOrWhiteSpace(name) ? typeof(TState).Name : name;
        var filePath = Path.Combine(_statesDirectory, $"{stateName}.json");
        return filePath;
    }
}
