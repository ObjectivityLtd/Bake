using Cake.CD.Scripting;
using Cake.CD.Templating;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.CD.Templating.Steps.Build
{
    public class BuildScriptState : IScriptState
    {
        public List<IScriptTask> ScriptTasks { get; private set; }

        public IScriptTask CurrentTask { get; set; }

        public DirectoryPath BasePath { get; private set; }

        public DirectoryPath CakeScriptPath { get; private set; }

        public DirectoryPath OutputPath { get; private set; }

        private ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, string cakeScriptPath, string outputPath)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.ScriptTasks = new List<IScriptTask>();
            this.BasePath = new DirectoryPath(Directory.GetCurrentDirectory());
            this.CakeScriptPath = this.BasePath.Combine(cakeScriptPath);
            this.OutputPath = this.BasePath.Combine(outputPath);
        }

        public void AddScriptTask(IScriptTask scriptTask)
        {
            this.ScriptTasks.Add(scriptTask);
        }

        public string GenerateParts(ScriptTaskPart scriptTaskPart)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var task in ScriptTasks)
            {
                this.CurrentTask = task;
                var part = scriptTaskEvaluator.GeneratePart(task, scriptTaskPart, this);
                sb.AppendLine(part);
            }

            return sb.ToString();
        }

/*        public string GetRelativePath(string path)
        {
            var filePath = new FilePath(path);
            if (filePath.IsRelative)
            {
                return path;
            } else
            {
                var baseDir = new DirectoryPath(this.BasePath);
                return baseDir.GetRelativePath(filePath).FullPath;
            }
        }*/
    }
}
