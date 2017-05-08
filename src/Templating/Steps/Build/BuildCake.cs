using Serilog;
using System.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class BuildCake : IScriptTask, ITemplatePlanStep
    {
        private readonly BuildScriptState scriptState;

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        public string Name
        {
            get
            {
                return "build.cake";
            }
        }
       
        public BuildCake(ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.scriptState = new BuildScriptState(scriptTaskEvaluator, "build", "bin");
        }

        public BuildCake AddScriptTask(IScriptTask scriptTask)
        {
            this.scriptState.AddScriptTask(scriptTask);
            return this;
        }

        public TemplatePlanStepResult Execute()
        {
            var buildCakePath = scriptState.CakeScriptPath.CombineWithFilePath("build.cake").FullPath;
            if (File.Exists(buildCakePath))
            {
                Log.Warning("File {BuildCakePath} already exists. Skipping.", buildCakePath);
                return new TemplatePlanStepResult();
            }

            Log.Information("Generating {Name}.", this.Name);
            var result = scriptTaskEvaluator.GenerateAllParts(this, scriptState);
            Log.Information("Saving result to {OutputPath}", buildCakePath);
            File.WriteAllText(buildCakePath, result);
            return new TemplatePlanStepResult(buildCakePath);
        }
    }
}
