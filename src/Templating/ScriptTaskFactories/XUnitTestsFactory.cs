using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class XUnitTestsFactory : IScriptTaskFactory
    {
        public int Order => 10;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.FindReference("xunit") != null;
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new XUnitTestsTask(projectInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }  
    }
}
