using Cake.CD.Scripting;
using Serilog;
using System.IO;

namespace Cake.CD.Templating.Build
{
    public class BuildCake : IScriptTask, ITemplatePlanStep
    {
        private static readonly string OUTPUT_PATH = "build\\build.cake";

        private readonly BuildScriptState scriptState;

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;
       
        public BuildCake(ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.scriptState = new BuildScriptState();
        }

        public BuildCake AddTaskScript(IScriptTask taskScript)
        {
            this.scriptState.AddTaskScript(taskScript);
            return this;
        }

        public TemplatePlanStepResult Execute()
        {
            if (File.Exists(OUTPUT_PATH))
            {
                Log.Warning("File {OutputPath} already exists. Skipping.", OUTPUT_PATH);
                return new TemplatePlanStepResult();
            }

            Log.Information("Evaluating build template script");
            var result = scriptTaskEvaluator.GenerateBuildScript(this, scriptState);
            Log.Information("Saving result to {OutputPath}", OUTPUT_PATH);
            File.WriteAllText(OUTPUT_PATH, result);
            return new TemplatePlanStepResult(OUTPUT_PATH);
        }
    }
}
