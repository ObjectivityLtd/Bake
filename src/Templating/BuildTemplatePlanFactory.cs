using System.Collections.Generic;
using System.IO;
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
            this.scriptTaskFactories = scriptTaskFactories;
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
            Log.Information("Parsing solution {SlnFile}.", slnFilePath);
            LogHelper.IncreaseIndent();
            var solutionParserResult = solutionParser.Parse(slnFilePath);
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                var scriptTasks = ParseProject(project);
                buildCakeTask.AddScriptTasks(scriptTasks);
            }
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

        private IEnumerable<IScriptTask> ParseProject(SolutionProject project)
        {
            Log.Information("Parsing project {ProjectFile}.", project.Path.FullPath);
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
                return CreateScriptTasks(project, projectParserResult);
            }
            finally
            {
                LogHelper.DecreaseIndent();
            }
        }

        private IEnumerable<IScriptTask> CreateScriptTasks(SolutionProject project, ProjectParserResult parserResult)
        {
            var result = new List<IScriptTask>();
            foreach (var scriptTaskFactory in scriptTaskFactories)
            {
                if (scriptTaskFactory.IsApplicable(project, parserResult))
                {
                    result.AddRange(scriptTaskFactory.Create(project, parserResult));
                }
            }
            return result;
        }
    }
}
