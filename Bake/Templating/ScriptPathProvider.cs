using System.IO;
using System.Text.RegularExpressions;
using Bake.API.Task;

namespace Bake.Templating
{
    public class ScriptPathProvider
    {
        private static readonly Regex TaskRegex = new Regex(@"Task$");

        private static readonly string TemplatesFolder = "Templates";

        public string GetPath(ITask scriptTask, TaskPart taskPart)
        {
            return Path.Combine(
                this.GetFolder(scriptTask), 
                this.GetFileName(scriptTask, taskPart));
        }

        private string GetFolder(ITask scriptTask)
        {
            var ns = scriptTask.GetType().Namespace;
            var nsWithoutLastPart = ns.Substring(0, ns.LastIndexOf('.'));
            return Path.Combine(nsWithoutLastPart, TemplatesFolder);
        }

        private string GetFileName(ITask scriptTask, TaskPart taskPart)
        {
            var name = scriptTask.GetType().Name;
            return TaskRegex.Replace(name, "") + "_" + taskPart.ToString().ToLower() + ".csx";
        }
    }
}
