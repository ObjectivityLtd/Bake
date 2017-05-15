using Cake.CD.Templating.Steps.Build;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories.Project
{
    public class GulpFactory : AbstractNpmTaskFactory
    {
        public override int Order => 30;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsWebsite())
            {
                return false;
            }
            return (this.IsPackageJsonPresent(projectInfo.ProjectDirectoryPath)
                    && System.IO.File.Exists(projectInfo.ProjectDirectoryPath.CombineWithFilePath("gulpfile.js").FullPath));            
        }

        public override IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new GulpTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
