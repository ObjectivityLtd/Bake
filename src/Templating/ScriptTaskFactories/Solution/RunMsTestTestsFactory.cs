using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating.ScriptTaskFactories.Solution
{
    public class MsTestTestsFactory : AbstractSolutionScriptTaskFactory
    {
        public override bool IsApplicable(SolutionInfo solutionInfo)
        {
            return solutionInfo.Projects.Any(projectInfo =>
                projectInfo.ParserResult != null
                && projectInfo.ParserResult.IsTestProjectType(projectInfo.Project.Path.FullPath)
                && projectInfo.ParserResult.IsMsTestProject(projectInfo.Project.Path.FullPath)
                && projectInfo.FindReference("nunit") == null
                && projectInfo.FindReference("xunit") == null);
        }

        public override IEnumerable<IScriptTask> Create(SolutionInfo solutionInfo)
        {
            return new List<IScriptTask>
            {
                new MsTestTestsTask(solutionInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }

        
    }
}
