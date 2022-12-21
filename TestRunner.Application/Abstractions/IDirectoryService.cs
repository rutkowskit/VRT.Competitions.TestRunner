namespace VRT.Competitions.TestRunner.Application.Abstractions;
public interface IDirectoryService
{
    string[] GetFiles(string directory, string pattern, SearchOption options);
}
