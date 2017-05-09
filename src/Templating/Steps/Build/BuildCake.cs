using Cake.CD.Command;
using Serilog;
using System.Collections.Generic;
using System.IO;
using Cake.CD.Logging;

namespace Cake.CD.Templating.Steps.Build
{
    public class BuildCake : ITemplatePlanStep, IScriptTask
    {
        private readonly CakeBuildScriptState scriptState;

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        private readonly InitOptions initOptions;

        public string Name => "build.cake";

        public BuildCake(ScriptTaskEvaluator scriptTaskEvaluator, InitOptions initOptions)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.initOptions = initOptions;
            this.scriptState = new CakeBuildScriptState(scriptTaskEvaluator, "build", "bin");
        }

        public BuildCake AddScriptTasks(IEnumerable<IScriptTask> scriptTasks)
        {
            this.scriptState.AddScriptTasks(scriptTasks);
            return this;
        }

        public TemplatePlanStepResult Execute()
        {
            var buildCakePath = scriptState.BuildScriptPath.CombineWithFilePath("build.cake").FullPath;
            if (!initOptions.Overwrite && File.Exists(buildCakePath))
            {
                Log.Warning("File {BuildCakePath} already exists. Skipping.", buildCakePath);
                return new TemplatePlanStepResult();
            }

            Log.Information("Generating {Name}.", this.Name);
            LogHelper.IncreaseIndent();
            var result = scriptTaskEvaluator.GenerateAllParts(this, scriptState);
            Log.Information("Saving result to {OutputPath}.", buildCakePath);
            File.WriteAllText(buildCakePath, result);
            LogHelper.DecreaseIndent();
            return new TemplatePlanStepResult(buildCakePath);
        }
    }
}
