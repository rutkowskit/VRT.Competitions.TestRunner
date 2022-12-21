using VRT.Competitions.TestRunner.Application.Abstractions;
using VRT.Competitions.TestRunner.Application.TestTasks.States;

namespace VRT.Competitions.TestRunner.Application.TestTasks;
public interface ITestTaskContext: ITestTaskWithState
{
    IShellService Shell { get; }
}
