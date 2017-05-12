namespace Cake.CD.Templating.Steps.Build
{
    public class CleanTask : IScriptTask
    {
        public string Name => "Clean";

        public ScriptTaskType Type => ScriptTaskType.BuildBackend;
    }
}
