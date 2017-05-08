using System.IO;
using System.Text.RegularExpressions;

namespace Cake.CD.Templating
{
    public class ScriptPathProvider
    {
        private static Regex TASK_REGEX = new Regex(@"Task$");

        public string GetPath(IScriptTask scriptTask, ScriptTaskPart scriptTaskPart)
        {
            return Path.Combine(
                this.GetFolder(scriptTask), 
                this.GetFileName(scriptTask, scriptTaskPart));
        }

        private string GetFolder(IScriptTask scriptTask)
        {
            var ns = scriptTask.GetType().Namespace;
            return ns.Substring(ns.LastIndexOf('.') + 1);
        }

        private string GetFileName(IScriptTask scriptTask, ScriptTaskPart scriptTaskPart)
        {
            var name = scriptTask.GetType().Name;
            return TASK_REGEX.Replace(name, "") + "_" + scriptTaskPart.ToString().ToLower() + ".csx";
        }
    }
}
