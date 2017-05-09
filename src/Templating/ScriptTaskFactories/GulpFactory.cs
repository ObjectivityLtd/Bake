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
        public bool IsApplicable(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            var isWebsite = MsBuildGuids.IsWebSite(solutionProject.Type);
            if (!isWebsite)
            {
                return false;
            }
            var projectDir = new DirectoryPath(solutionProject.Path.FullPath);
            return (System.IO.File.Exists(projectDir.CombineWithFilePath("package.json").FullPath)
                    && System.IO.File.Exists(projectDir.CombineWithFilePath("gulpfile.js").FullPath));            
        }

        public IEnumerable<IScriptTask> Create(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            Log.Information("Project {ProjectFile} is website with npm and gulp files - adding gulp template.", solutionProject.Path.GetFilename());
            return new List<IScriptTask>
            {
                new GulpTask(new DirectoryPath(solutionProject.Path.FullPath))
            };
        }
    }
}
