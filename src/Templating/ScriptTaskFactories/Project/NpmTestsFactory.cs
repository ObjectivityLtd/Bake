using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Project
{
    public class NpmTestsFactory : AbstractNpmTaskFactory
    {
        public override int ParsingOrder => 31;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsWebsite() 
                && this.IsPackageJsonPresent(projectInfo.ProjectDirectoryPath) 
                && this.IsNpmTestScriptPresent(projectInfo.ProjectDirectoryPath);
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new NpmTestsTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
