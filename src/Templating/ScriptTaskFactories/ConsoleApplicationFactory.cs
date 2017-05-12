using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class ConsoleApplicationFactory : IScriptTaskFactory
    {
        public int ParsingOrder => 10;

        public bool IsTerminating => false;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsExecutableApplication();
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new MsBuildTask(MsBuildTask.MsBuildTaskType.ConsoleApplication, projectInfo.Project.Path, projectInfo.Project.Name)
            };
        }
    }
}
