using Cake.CD.Templating.Steps.Build;
using Cake.Core.IO;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class GulpFactory : AbstractNpmTaskFactory, IScriptTaskFactory
    {
        public int ParsingOrder => 10;

        public bool IsTerminating => false;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsWebsite())
            {
                return false;
            }
            return (this.IsPackageJsonPresent(projectInfo.ProjectDirectoryPath)
                    && System.IO.File.Exists(projectInfo.ProjectDirectoryPath.CombineWithFilePath("gulpfile.js").FullPath));            
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new GulpTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
