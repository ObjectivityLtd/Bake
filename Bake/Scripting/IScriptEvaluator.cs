using Bake.API.Task;

namespace Bake.Scripting
{
    public interface IScriptEvaluator
    {
        string Evaluate(ITask task, IScriptState scriptState, string scriptBody);
    }
}
