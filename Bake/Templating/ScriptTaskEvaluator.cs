using Bake.API.EntryScript;
using Bake.API.Logging;
using Bake.API.Task;
using Bake.Scripting;

namespace Bake.Templating
{
    public class ScriptTaskEvaluator : IScriptTaskEvaluator
    {
        private readonly IScriptEvaluator scriptEvaluator;

        private readonly TemplateFileProvider templateFileProvider;

        private readonly ScriptPathProvider scriptPathProvider;

        public ScriptTaskEvaluator(IScriptEvaluator scriptEvaluator, TemplateFileProvider templateFileProvider, ScriptPathProvider scriptPathProvider)
        {
            this.scriptEvaluator = scriptEvaluator;
            this.templateFileProvider = templateFileProvider;
            this.scriptPathProvider = scriptPathProvider;
        }

        public string GenerateAllParts(ITask scriptTask, IScriptState scriptState)
        {
            return string.Concat(
                GeneratePart(scriptTask, TaskPart.Header, scriptState),
                GeneratePart(scriptTask, TaskPart.Body, scriptState),
                GeneratePart(scriptTask, TaskPart.Footer, scriptState)
                );
        }

        public string GeneratePart(ITask scriptTask, TaskPart TaskPart, IScriptState scriptState)
        {
            var scriptPath = scriptPathProvider.GetPath(scriptTask, TaskPart);
            var isTemplateOptional = TaskPart != TaskPart.Body;
            var scriptBody = templateFileProvider.GetFileContents(scriptPath, isTemplateOptional);
            if (scriptBody == null)
            {
                return null;
            }
            try
            {
                scriptState.CurrentTask = scriptTask;
                Log.Info("Generating {TaskPart} of {Type} '{ScriptTaskName}'.", 
                    TaskPart.ToString(), scriptTask.GetType().Name, scriptTask.Name);
                Log.IncreaseIndent();
                return scriptEvaluator.Evaluate(scriptTask, scriptState, scriptBody);
            }
            finally
            {
                scriptState.CurrentTask = null;
                Log.DecreaseIndent();
            }
            
        }

    }
}
