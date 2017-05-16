using System.Collections.Generic;
using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Solution
{
    public class CleanTaskFactory : AbstractSolutionScriptTaskFactory
    {
        public override int Order => 0;

        public override bool IsApplicable(SolutionInfo projectInfo)
        {
            return true;
        }

        public override IEnumerable<ITask> Create(SolutionInfo solutionInfo)
        {
            return new List<ITask>
            {
                new CleanTask()
            };
        }

        
    }
}
