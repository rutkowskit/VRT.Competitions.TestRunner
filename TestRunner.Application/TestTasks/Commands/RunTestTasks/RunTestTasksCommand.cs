using CSharpFunctionalExtensions;
using MediatR;
using VRT.Competitions.TestRunner.Application.Abstractions;

namespace VRT.Competitions.TestRunner.Application.TestTasks.Commands.RunTestTasks;
public sealed record class RunTestTasksCommand(IReadOnlyCollection<ITestTaskWithState> TestTasks) : IRequest<Result>
{
    public Action<(int Count, int Total)>? OnProgressCallback { get; init; }
    public Action<ITestTaskWithState>? OnBeforeRunTestCallback { get; init; }

    internal sealed class RunTestTasksCommandHandler : IRequestHandler<RunTestTasksCommand, Result>
    {
        private readonly IShellService _shellService;

        public RunTestTasksCommandHandler(IShellService shellService)
        {
            _shellService = shellService;
        }
        public async Task<Result> Handle(RunTestTasksCommand request, CancellationToken cancellationToken)
        {
            var result = await Result.Success(request)
                .Ensure(r => r.TestTasks.Count > 0, "No tasks to run")
                .Tap(r => ExecuteTasks(r, cancellationToken));
            return result;
        }
        private async Task ExecuteTasks(RunTestTasksCommand request, CancellationToken cancellationToken)
        {
            int cnt = 0;
            int total = request.TestTasks.Count;
            var runContexts = request.TestTasks.Select(ToTestTaskRunContext).ToArray();
            foreach (var runContext in runContexts)
            {
                request.OnBeforeRunTestCallback?.Invoke(runContext.DecoratedTestTask);
                if (cancellationToken.IsCancellationRequested) break;
                await runContext.StartAsync(cancellationToken);
                cnt++;
                request.OnProgressCallback?.Invoke((cnt, total));
            }
        }
        private TestTaskRunContext ToTestTaskRunContext(ITestTaskWithState testTask)
        {
            var result = new TestTaskRunContext(testTask)
            {
                Shell = _shellService
            };
            return result;
        }
    }
}
