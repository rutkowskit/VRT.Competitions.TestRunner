using CSharpFunctionalExtensions;
using MediatR;
using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Application.Extensions;

namespace VRT.Competitions.TestRunner.Application.TestTasks.Queries.GetTestTasks;
public sealed record GetTestTasksQuery(string ExecutableFilePath, string TestsDirectoryPath) : IRequest<Result<TestTaskParams[]>>
{
    internal sealed class GetTestTasksQueryHandler : IRequestHandler<GetTestTasksQuery, Result<TestTaskParams[]>>
    {
        private readonly IDirectoryService _directoryService;

        public GetTestTasksQueryHandler(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }
        public async Task<Result<TestTaskParams[]>> Handle(GetTestTasksQuery request, CancellationToken cancellationToken)
        {
            var inFiles = _directoryService
                .GetFiles(request.TestsDirectoryPath, "*.in", SearchOption.AllDirectories);
            var outFiles = _directoryService
                .GetFiles(request.TestsDirectoryPath, "*.out", SearchOption.AllDirectories);
            await Task.CompletedTask;

            var result = inFiles
                .Join(outFiles, ToFileNameKey, ToFileNameKey, (inFile, outFile) => (inFile, outFile))
                .Where(t => string.IsNullOrEmpty(t.inFile) is false)
                .Distinct()
                .Select(files => CreateTestTaskViewModelParams(request.ExecutableFilePath, files.inFile, files.outFile))
                .OrderBy(o => o.Priority)
                .ThenBy(o => o.InputFilePath)
                .ToArray();
            return result;
        }
        private static TestTaskParams CreateTestTaskViewModelParams(
            string executableFilePath,
            string inFile,
            string outFile)
        {
            return new TestTaskParams()
            {
                ExecutableFilePath = executableFilePath!,
                InputFilePath = inFile,
                OutputFilePath = outFile,
                Priority = inFile.TryGetPriority(out var priority) ? priority : 0
            };
        }
        private static string ToFileNameKey(string fullFilePath)
            => Path.GetFileNameWithoutExtension(fullFilePath);
    }
}
