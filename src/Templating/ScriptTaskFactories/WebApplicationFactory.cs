using System.Collections.Generic;
using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class WebApplicationFactory : IScriptTaskFactory
    {
        public bool IsApplicable(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            return parserResult != null && parserResult.IsWebApplication(solutionProject.Path.FullPath);
        }

        public IEnumerable<IScriptTask> Create(SolutionProject solutionProject, ProjectParserResult parserResult)
        {
            Log.Information("Project {ProjectFile} is web application - adding msbuild template.", solutionProject.Path.GetFilename());
            var sourceFile = solutionProject.Path;
            var solutionName = solutionProject.Name;
            return new List<IScriptTask>
            {
                new RestoreNuGetTask(sourceFile),
                new MsBuildTask(MsBuildTask.MsBuildTaskType.WebApplication, sourceFile, solutionName)
            };
        }
    }
}
