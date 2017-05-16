using Bake.API.Task;

namespace Bake.API.EntryScript
{
    public interface IScriptTaskEvaluator
    {
        string GenerateAllParts(ITask scriptTask, IScriptState scriptState);

        string GeneratePart(ITask scriptTask, TaskPart taskPart, IScriptState scriptState);
    }
}
