namespace Cake.CD.Templating
{
    public interface IScriptTask
    {
        string Name { get; }

        ScriptTaskType Type { get; }

    }
}
