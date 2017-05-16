using Bake.API.Task;
using Bake.API.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bake.API.EntryScript;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.EntryScript
{
    public class BuildCake : ITemplatePlanStep, ITask
    {
        private readonly CakeBuildScriptState scriptState;

        private readonly IScriptTaskEvaluator scriptTaskEvaluator;

        private readonly InitOptions initOptions;

        public string Name => "build.cake";

        public TaskType Type => TaskType.Entry;

        public bool AutoParameterizeWebConfig => false;

        public bool HasMsBuildSteps => scriptState.ScriptTasks.Any(scriptTask => scriptTask is MsBuildTask);

        public BuildCake(IScriptTaskEvaluator scriptTaskEvaluator, InitOptions initOptions)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.initOptions = initOptions;
            this.scriptState = new CakeBuildScriptState(scriptTaskEvaluator, initOptions.SolutionFilePath, "build", "bin");
        }

        public BuildCake AddScriptTasks(IEnumerable<ITask> scriptTasks)
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
                Log.Warn("File {BuildCakePath} already exists. Skipping.", buildCakePath);
                return new TemplatePlanStepResult();
            }

            Log.Info("Generating {Name}.", this.Name);
            Log.IncreaseIndent();
            var result = scriptTaskEvaluator.GenerateAllParts(this, scriptState);
            Log.Info("{Creating} file {File}.", fileExists ? "Overwriting" : "Creating", buildCakePath);
            File.WriteAllText(buildCakePath, result);
            Log.DecreaseIndent();
            return new TemplatePlanStepResult(buildCakePath);
        }
    }
}
