using Cake.CD.Scripting;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.CD.Templating.Steps.Build
{
    public abstract class BuildScriptState : IScriptState
    {
        public List<IScriptTask> ScriptTasks { get; }

        public IScriptTask CurrentTask { get; set; }

        public DirectoryPath BasePath { get; }

        public FilePath SolutionFilePath { get; }

        public DirectoryPath BuildScriptPath { get; }

        public DirectoryPath OutputPath { get; }

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, FilePath solutionFilePath, DirectoryPath buildScriptPath, DirectoryPath outputPath)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.ScriptTasks = new List<IScriptTask>();
            this.SolutionFilePath = solutionFilePath;
            this.BasePath = new DirectoryPath(Directory.GetCurrentDirectory());
            this.BuildScriptPath = this.BasePath.Combine(buildScriptPath);
            this.OutputPath = this.BasePath.Combine(outputPath);
        }

        public void AddScriptTasks(IEnumerable<IScriptTask> scriptTasks)
        {
            this.ScriptTasks.AddRange(scriptTasks);
        }

        public string GenerateParts(ScriptTaskPart scriptTaskPart)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var task in ScriptTasks)
            {
                this.CurrentTask = task;
                var part = scriptTaskEvaluator.GeneratePart(task, scriptTaskPart, this);
                if (part != null)
                {
                    sb.AppendLine(part);
                }
            }

            return sb.ToString();
        }

    }
}
