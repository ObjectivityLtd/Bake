namespace Cake.CD.Templating.Steps.Build
{
    public abstract class AbstractBackendTestsTask : IScriptTask
    {
        public string Name => "Run tests " + this.SolutionName;

        public ScriptTaskType Type => ScriptTaskType.UnitTestBackend;

        public string SolutionName { get; }

        public AbstractBackendTestsTask(string solutionName)
        {
            this.SolutionName = solutionName;
        }
    }
}
