using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using System.Collections.Generic;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Project
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

        public override IEnumerable<ITask> Create(ProjectInfo projectInfo)
        {
            return new List<ITask>
            {
                new GulpTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
