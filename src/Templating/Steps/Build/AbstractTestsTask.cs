namespace Cake.CD.Templating.Steps.Build
{
    public abstract class AbstractTestsTask : IScriptTask
    {
        public string Name => "Run tests " + this.SolutionName;

        public ScriptTaskType Type => ScriptTaskType.UNIT_TEST_BACKEND;

        public string SolutionName { get; }

        public AbstractTestsTask(string solutionName)
        {
            this.SolutionName = solutionName;
        }
    }
}
