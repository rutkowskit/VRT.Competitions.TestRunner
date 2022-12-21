using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Infrastructure.Services;
public sealed class DirectoryService : IDirectoryService
{
    public string[] GetFiles(string directory, string pattern, SearchOption options)
    {
        return Directory.GetFiles(directory, pattern, options);
    }
}
