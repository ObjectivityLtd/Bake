using Cake.CD.Scripting;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cake.CD.Templating.Steps.Build
{
    public abstract class BuildScriptState : IScriptState
    {
        public List<IScriptTask> ScriptTasks { get; private set; }

        public IScriptTask CurrentTask { get; set; }

        public DirectoryPath BasePath { get; private set; }

        public DirectoryPath BuildScriptPath { get; private set; }

        public DirectoryPath OutputPath { get; private set; }

        private readonly ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, string buildScriptPath, string outputPath)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.ScriptTasks = new List<IScriptTask>();
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
                sb.AppendLine(part);
            }

            return sb.ToString();
        }

        public IEnumerable<string> GetDependencyList()
        {
            return this.ScriptTasks.Select(task => task.Name).ToList();
        }

    }
}
