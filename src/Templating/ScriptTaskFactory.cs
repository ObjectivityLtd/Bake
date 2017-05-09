using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;
using System.Collections.Generic;
using Cake.CD.Logging;

namespace Cake.CD.Templating
{
    public class ScriptTaskFactory
    {
        private readonly ProjectParser projectParser;

        public ScriptTaskFactory(ProjectParser projectParser)
        {
            this.projectParser = projectParser;
        }

        public List<IScriptTask> CreateTaskTemplate(SolutionProject solutionProject)
        {
            Log.Information("Parsing project {ProjectFile}.", solutionProject.Path.FullPath, solutionProject.Type);
            try
            {
                LogHelper.IncreaseIndent();
                return ParseSolution(solutionProject);
            }
            finally
            {
                LogHelper.DecreaseIndent();
            }
        }

        private List<IScriptTask> ParseSolution(SolutionProject solutionProject)
        {
            var projectParserResult = projectParser.Parse(solutionProject.Path);
            if (projectParserResult.IsWebApplication(solutionProject.Path.FullPath))
            {
                Log.Information("Project {ProjectFile} is web application - adding msbuild template.",
                    solutionProject.Path.GetFilename());
                return CreateMsBuildTask(MsBuildTask.MsBuildTaskType.WebApplication, solutionProject.Path,
                    solutionProject.Name);
            }
            if (projectParserResult.IsExecutableApplication())
            {
                Log.Information("Project {ProjectFile} is executable application - adding msbuild template.",
                    solutionProject.Path.FullPath);
                return CreateMsBuildTask(MsBuildTask.MsBuildTaskType.ConsoleApplication, solutionProject.Path,
                    solutionProject.Name);
            }
            // TODO: migrations project
            return null;
        }

        private List<IScriptTask> CreateMsBuildTask(MsBuildTask.MsBuildTaskType type, FilePath sourceFile,
            string solutionName)
        {
            return new List<IScriptTask>
            {
                new RestoreNuGetTask(sourceFile),
                new MsBuildTask(type, sourceFile, solutionName)
            };
        }
    }
}