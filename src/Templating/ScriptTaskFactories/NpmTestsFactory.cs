using Cake.CD.Templating.Steps.Build;
using Cake.Core.IO;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class NpmTestsFactory : AbstractNpmTaskFactory, IScriptTaskFactory
    {
        public int ParsingOrder => 10;

        public bool IsTerminating => false;

        public bool IsApplicable(ProjectInfo projectInfo)
        {
            return projectInfo.IsWebsite() 
                && this.IsPackageJsonPresent(projectInfo.ProjectDirectoryPath) 
                && this.IsNpmTestScriptPresent(projectInfo.ProjectDirectoryPath);
        }

        public IEnumerable<IScriptTask> Create(ProjectInfo projectInfo)
        {
            return new List<IScriptTask>
            {
                new NpmTestsTask(projectInfo.ProjectDirectoryPath, GetNpmProjectName(projectInfo.ProjectDirectoryPath))
            };
        }
    }
}
