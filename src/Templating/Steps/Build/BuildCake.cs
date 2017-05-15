using Cake.CD.Command;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.CD.Logging;

namespace Cake.CD.Templating.Steps.Build
{
    public class BuildCake : ITemplatePlanStep, IScriptTask
    {
        private readonly CakeBuildScriptState scriptState;

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        private readonly InitOptions initOptions;

        public string Name => "build.cake";

        public ScriptTaskType Type => ScriptTaskType.Entry;

        public bool AutoParameterizeWebConfig => false;

        public bool HasMsBuildSteps => scriptState.ScriptTasks.Any(scriptTask => scriptTask is MsBuildTask);

        public BuildCake(ScriptTaskEvaluator scriptTaskEvaluator, InitOptions initOptions)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.initOptions = initOptions;
            this.scriptState = new CakeBuildScriptState(scriptTaskEvaluator, initOptions.SolutionFilePath, "build", "bin");
        }

        public BuildCake AddScriptTasks(IEnumerable<IScriptTask> scriptTasks)
        {
            this.scriptState.AddScriptTasks(scriptTasks);
            return this;
        }

        public TemplatePlanStepResult Execute()
        {
            var buildCakePath = scriptState.BuildScriptPath.CombineWithFilePath("build.cake").FullPath;
            var fileExists = File.Exists(buildCakePath);
            if (!initOptions.Overwrite && fileExists)
            {
                Log.Warning("File {BuildCakePath} already exists. Skipping.", buildCakePath);
                return new TemplatePlanStepResult();
            }

            Log.Information("Generating {Name}.", this.Name);
            LogHelper.IncreaseIndent();
            var result = scriptTaskEvaluator.GenerateAllParts(this, scriptState);
            Log.Information("{Creating} file {File}.", fileExists ? "Overwriting" : "Creating", buildCakePath);
            File.WriteAllText(buildCakePath, result);
            LogHelper.DecreaseIndent();
            return new TemplatePlanStepResult(buildCakePath);
        }
    }
}
