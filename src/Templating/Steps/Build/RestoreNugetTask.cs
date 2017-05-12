namespace Cake.CD.Templating.Steps.Build
{
    public class RestoreNugetTask : IScriptTask
    {
        public string Name => "Restore nuget " + this.SolutionName;

        public ScriptTaskType Type => ScriptTaskType.BuildBackend;

        public string SolutionName { get; }

        public RestoreNugetTask(string solutionName)
        {
            this.SolutionName = solutionName;
        }
    }
}
