using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.CD.Command;
using Cake.CD.Logging;
using Cake.CD.MsBuild;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.CD.Templating.Steps;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;
using Serilog.Context;

namespace Cake.CD.Templating
{
    public class BuildTemplatePlanFactory
    {

        private SolutionParser solutionParser;

        private ProjectParser projectParser;

        private IEnumerable<IScriptTaskFactory> scriptTaskFactories;

        private TemplateFileProvider templateFileProvider;

        private ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildTemplatePlanFactory(SolutionParser solutionParser, ProjectParser projectParser, IEnumerable<IScriptTaskFactory> scriptTaskFactories,
            TemplateFileProvider templateFileProvider, ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.scriptTaskFactories = scriptTaskFactories.OrderBy(stf => stf.ParsingOrder).ToList();
            this.templateFileProvider = templateFileProvider;
            this.scriptTaskEvaluator = scriptTaskEvaluator;
        }

        public TemplatePlan CreateTemplatePlan(InitOptions initOptions)
        {
            var buildCakeTask = new BuildCake(scriptTaskEvaluator, initOptions);
            var templatePlan = this.CreateBaseTemplatePlan(buildCakeTask, initOptions.Overwrite);
            if (initOptions.SolutionFilePath == null)
            {
                Log.Warning("No solution file provided - creating default templates.");
                return templatePlan;
            }
            this.ParseSolution(buildCakeTask, initOptions.SolutionFilePath);
            return templatePlan;
        }

        private BuildCake ParseSolution(BuildCake buildCakeTask, FilePath slnFilePath)
        {
            Log.Information("Parsing solution {SlnFile}.", new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(slnFilePath));
            LogHelper.IncreaseIndent();
            var solutionParserResult = solutionParser.Parse(slnFilePath);
            var buildCakeScriptTasks = new List<IScriptTask>();
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                var scriptTasks = ParseProject(slnFilePath, project);
                var uniqueScriptTasks = GetNewUniqueScriptTasks(buildCakeScriptTasks, scriptTasks);
                buildCakeScriptTasks.AddRange(uniqueScriptTasks);
            }
            var orderedTasks = buildCakeScriptTasks.OrderBy(task => task.Type.TaskOrder);
            buildCakeTask.AddScriptTasks(orderedTasks);
            LogHelper.DecreaseIndent();
            return buildCakeTask;
        }

        private TemplatePlan CreateBaseTemplatePlan(BuildCake buildCakeTask, bool shouldOverwrite)
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan.AddStep(new CopyFileStep(templateFileProvider, "build\\build.ps1", "build\\build.ps1", shouldOverwrite));
            templatePlan.AddStep(buildCakeTask);
            return templatePlan;
        }

        private IEnumerable<IScriptTask> ParseProject(FilePath solutionFilePath, SolutionProject project)
        {
            Log.Information("Parsing project {ProjectFile}.", new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(project.Path));
            LogHelper.IncreaseIndent();
            try
            {
                ProjectParserResult projectParserResult = null;
                if (File.Exists(project.Path.FullPath))
                {
                    projectParserResult = projectParser.Parse(project.Path);
                }
                else if (!Directory.Exists(project.Path.FullPath))
                {
                    Log.Warning("Project points to a non-existing file or directory: {Path}.", project.Path.FullPath);
                    return new List<IScriptTask>();
                }
                var projectInfo = new ProjectInfo(solutionFilePath, project, projectParserResult);
                return CreateScriptTasks(projectInfo);
            }
            finally
            {
                LogHelper.DecreaseIndent();
            }
        }

        private IEnumerable<IScriptTask> CreateScriptTasks(ProjectInfo projectInfo)
        {
            var result = new List<IScriptTask>();
            foreach (var scriptTaskFactory in scriptTaskFactories.Where(stf => stf.IsApplicable(projectInfo)))
            {
                var projectType = scriptTaskFactory.GetType().Name.Replace("Factory", "");
                Log.Information("Recognized project to be {ProjectType}.", projectType);
                var scriptTasks = scriptTaskFactory.Create(projectInfo);
                result.AddRange(scriptTasks);
                if (scriptTaskFactory.IsTerminating)
                {
                    break;
                }
            }
            return result;
        }

        private IEnumerable<IScriptTask> GetNewUniqueScriptTasks(IEnumerable<IScriptTask> existingScriptTasks, IEnumerable<IScriptTask> newScriptTasks)
        {
            var existingNames = existingScriptTasks.Select(scriptTask => scriptTask.Name).ToList();
            return newScriptTasks.Where(scriptTask => !existingNames.Contains(scriptTask.Name));
        }
    }
}
