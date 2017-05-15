using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class MsTestTestsFactory : AbstractScriptTaskFactory
    {
        public override bool IsSolutionLevel => true;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            var parserResult = projectInfo.ParserResult;
            return (parserResult != null
                    && parserResult.IsTestProjectType(projectInfo.Project.Path.FullPath)
                    && parserResult.IsMsTestProject(projectInfo.Project.Path.FullPath)
                    && projectInfo.FindReference("nunit") == null
                    && projectInfo.FindReference("xunit") == null
            );
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new MsTestTestsTask(projectInfo.SolutionFilePath.GetFilenameWithoutExtension().FullPath)
            };
        }

        
    }
}
