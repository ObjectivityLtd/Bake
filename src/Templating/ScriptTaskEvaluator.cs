using Cake.CD.Scripting;

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
                this.GeneratePart(scriptTask, ScriptTaskPart.HEADER, scriptState),
                this.GeneratePart(scriptTask, ScriptTaskPart.BODY, scriptState),
                this.GeneratePart(scriptTask, ScriptTaskPart.FOOTER, scriptState)
                );
        }

        public string GeneratePart(IScriptTask scriptTask, ScriptTaskPart scriptTaskPart, IScriptState scriptState)
        {
            var scriptPath = scriptPathProvider.GetPath(scriptTask, scriptTaskPart);
            var scriptBody = templateFileProvider.GetOptionalFileContents(scriptPath);
            if (scriptBody == null)
            {
                return null;
            }
            try
            {
                scriptState.CurrentTask = scriptTask;
                return this.scriptEvaluator.Evaluate(scriptBody, scriptState);
            }
            finally
            {
                scriptState.CurrentTask = null;
            }
            
        }

    }
}
