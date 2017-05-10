using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cake.CD.Templating.Steps.Build
{
    public class CakeBuildScriptState : BuildScriptState
    {
        private readonly ISet<string> cakeAddins = new HashSet<string>();

        public CakeBuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, string buildScriptPath, string outputPath)
            : base(scriptTaskEvaluator, buildScriptPath, outputPath)
        {
            
        }

        public string AddCakeAddin(string addinName)
        {
            if (cakeAddins.Contains(addinName))
            {
                return "";
            }
            cakeAddins.Add(addinName);
            return $"#addin \"{addinName}\"{Environment.NewLine}";
        }

        public string AddDefaultTask()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Task(\"Default\")");
            var dependencies = GetDependencyList();
            var dependenciesCount = dependencies.Count();
            int i = 0;
            foreach (var dependency in dependencies)
            {
                sb.Append($"    .IsDependentOn(\"{dependency}\")");
                if (++i == dependenciesCount)
                {
                    sb.Append(";");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private IEnumerable<string> GetDependencyList()
        {
            var result = new List<string>();
            result.Add("Clean");
            result.AddRange(this.ScriptTasks.Select(task => task.Name));
            return result;
        }
    }
}
