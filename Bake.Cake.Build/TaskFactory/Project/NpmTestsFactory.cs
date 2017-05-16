using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using System.Collections.Generic;
using Bake.Cake.Build.Task;

namespace Bake.Cake.Build.TaskFactory.Project
{
    public class NpmTestsFactory : AbstractNpmTaskFactory
    {
        public override int Order => 31;

        public override bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsWebsite() 
                && this.IsPackageJsonPresent(projectInfo.ProjectDirectoryPath) 
                && this.IsNpmTestScriptPresent(projectInfo.ProjectDirectoryPath);
        }

        public override IEnumerable<ITask> Create(ProjectInfo projectInfo)
        {
            return new List<ITask>
            {
                new NpmTestsTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
