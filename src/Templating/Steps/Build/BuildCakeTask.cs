using Cake.CD.Command;
using Serilog;
using System.IO;

namespace Cake.CD.Templating.Steps.Build
{
    public class BuildCakeTask : IScriptTask, ITemplatePlanStep
    {
        private readonly BuildScriptState scriptState;

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        private readonly InitOptions initOptions;

        public string Name
        {
            get
            {
                return "build.cake";
            }
        }
       
        public BuildCakeTask(ScriptTaskEvaluator scriptTaskEvaluator, InitOptions initOptions)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.initOptions = initOptions;
            this.scriptState = new BuildScriptState(scriptTaskEvaluator, "build", "bin");
        }

        public BuildCakeTask AddScriptTask(IScriptTask scriptTask)
        {
            this.scriptState.AddScriptTask(scriptTask);
            return this;
        }

        public TemplatePlanStepResult Execute()
        {
            var buildCakePath = scriptState.CakeScriptPath.CombineWithFilePath("build.cake").FullPath;
            if (!initOptions.Overwrite && File.Exists(buildCakePath))
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
