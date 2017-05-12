using System.Collections.Generic;
using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class GulpFactory : IScriptTaskFactory
    {
        public int Order => 10;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            if (!projectInfo.IsWebsite())
            {
                return false;
            }
            var projectDir = new DirectoryPath(projectInfo.Project.Path.FullPath);
            return (System.IO.File.Exists(projectDir.CombineWithFilePath("package.json").FullPath)
                    && System.IO.File.Exists(projectDir.CombineWithFilePath("gulpfile.js").FullPath));            
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new GulpTask(new DirectoryPath(projectInfo.Project.Path.FullPath))
            };
        }
    }
}
