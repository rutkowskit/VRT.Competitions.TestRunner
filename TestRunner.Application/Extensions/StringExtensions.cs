using System.Text.RegularExpressions;

namespace VRT.Competitions.TestRunner.Application.Extensions;
public static partial class StringExtensions
{
    public static bool TryGetPriority(this string inputFilePath, out int priority)
    {
        var fileName = (inputFilePath ?? "").Split('/','\\').LastOrDefault() ?? "";
        var numberMatch = MatchNumber().Match(fileName);        
        priority = numberMatch.Success
            ? int.Parse(numberMatch.Value)
            : 0;
        return numberMatch.Success;
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex MatchNumber();
}
