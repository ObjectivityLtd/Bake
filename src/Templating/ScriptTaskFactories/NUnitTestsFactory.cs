using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class NUnitTestsFactory : IScriptTaskFactory
    {
        public int Order => 10;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.FindReference("nunit") != null;
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new NUnitTestsTask(projectInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }

        
    }
}
