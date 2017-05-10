using Cake.CD.Logging;
using Cake.CD.Scripting;
using Serilog;

namespace Cake.CD.Templating
{
    public class ScriptTaskEvaluator
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

        public string GenerateAllParts(IScriptTask scriptTask, IScriptState scriptState)
        {
            return string.Concat(
                GeneratePart(scriptTask, ScriptTaskPart.HEADER, scriptState),
                GeneratePart(scriptTask, ScriptTaskPart.BODY, scriptState),
                GeneratePart(scriptTask, ScriptTaskPart.FOOTER, scriptState)
                );
        }

        public string GeneratePart(IScriptTask scriptTask, ScriptTaskPart scriptTaskPart, IScriptState scriptState)
        {
            var scriptPath = scriptPathProvider.GetPath(scriptTask, scriptTaskPart);
            var isTemplateOptional = scriptTaskPart != ScriptTaskPart.BODY;
            var scriptBody = templateFileProvider.GetFileContents(scriptPath, isTemplateOptional);
            if (scriptBody == null)
            {
                return null;
            }
            try
            {
                scriptState.CurrentTask = scriptTask;
                Log.Information("Generating {ScriptTaskPart} of {Type} '{ScriptTaskName}'.", 
                    scriptTaskPart, scriptTask.GetType().Name, scriptTask.Name);
                LogHelper.IncreaseIndent();
                return scriptEvaluator.Evaluate(scriptBody, scriptState);
            }
            finally
            {
                scriptState.CurrentTask = null;
                LogHelper.DecreaseIndent();
            }
            
        }

    }
}
