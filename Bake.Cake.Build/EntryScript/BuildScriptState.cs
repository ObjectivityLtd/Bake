using Bake.API.Task;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bake.API.EntryScript;

namespace Bake.Cake.Build.EntryScript
{
    public abstract class BuildScriptState : IScriptState
    {
        public List<ITask> ScriptTasks { get; }

        public ITask CurrentTask { get; set; }

        public DirectoryPath BasePath { get; }

        public FilePath SolutionFilePath { get; }

        public DirectoryPath BuildScriptPath { get; }

        public DirectoryPath OutputPath { get; }

        private readonly IScriptTaskEvaluator scriptTaskEvaluator;

        public BuildScriptState(IScriptTaskEvaluator scriptTaskEvaluator, FilePath solutionFilePath, DirectoryPath buildScriptPath, DirectoryPath outputPath)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.ScriptTasks = new List<ITask>();
            this.SolutionFilePath = solutionFilePath;
            this.BasePath = new DirectoryPath(Directory.GetCurrentDirectory());
            this.BuildScriptPath = this.BasePath.Combine(buildScriptPath);
            this.OutputPath = this.BasePath.Combine(outputPath);
        }

        public void AddScriptTasks(IEnumerable<ITask> scriptTasks)
        {
            this.ScriptTasks.AddRange(scriptTasks);
        }

        public string GenerateParts(TaskPart TaskPart, TaskType.Group TaskTypeGroup)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var task in ScriptTasks.Where(scriptTask => scriptTask.Type.TaskGroup == TaskTypeGroup))
            {
                this.CurrentTask = task;
                var part = scriptTaskEvaluator.GeneratePart(task, TaskPart, this);
                if (part != null)
                {
                    sb.AppendLine(part);
                }
            }

            return sb.ToString();
        }

    }
}
