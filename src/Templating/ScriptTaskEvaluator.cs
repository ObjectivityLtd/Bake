using Cake.CD.Scripting;

namespace Cake.CD.Templating
{
    public class ScriptTaskEvaluator
    {
        private readonly IScriptEvaluator scriptEvaluator;

        private readonly TemplateFileProvider templateFileProvider;

        public ScriptTaskEvaluator(IScriptEvaluator scriptEvaluator, TemplateFileProvider templateFileProvider)
        {
            this.scriptEvaluator = scriptEvaluator;
            this.templateFileProvider = templateFileProvider;
        }

        public string GenerateBuildScript(IScriptTask scriptTask, IScriptState scriptState)
        {
            var scriptPath = "build\\" + scriptTask.GetType().Name + ".csx";
            return this.Generate(scriptPath, scriptState);
        }

        private string Generate(string scriptPath, IScriptState scriptState)
        {
            var scriptBody = templateFileProvider.GetFileContents(scriptPath);
            return this.scriptEvaluator.Evaluate(scriptBody, scriptState);
        }
    }
}
