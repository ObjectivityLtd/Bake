namespace Cake.CD.Scripting
{
    public interface IScriptEvaluator
    {
        string Evaluate(string scriptBody, IScriptState scriptState);
    }
}
