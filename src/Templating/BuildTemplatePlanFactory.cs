using Cake.CD.Command;
using Cake.CD.Logging;
using Cake.CD.MsBuild;
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

        private ScriptTaskFactory scriptTaskFactory;

        private TemplateFileProvider templateFileProvider;

        private ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildTemplatePlanFactory(SolutionParser solutionParser, ProjectParser projectParser, ScriptTaskFactory scriptTaskFactory, 
            TemplateFileProvider templateFileProvider, ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.scriptTaskFactory = scriptTaskFactory;
            this.templateFileProvider = templateFileProvider;
            this.scriptTaskEvaluator = scriptTaskEvaluator;
        }

        public TemplatePlan CreateTemplatePlan(InitOptions initOptions)
        {
            var buildCakeTask = new BuildCakeTask(scriptTaskEvaluator, initOptions);
            var templatePlan = this.CreateBaseTemplatePlan(buildCakeTask, initOptions.Overwrite);
            if (initOptions.SolutionFilePath == null)
            {
                Log.Warning("No solution file provided - creating default templates.");
                return templatePlan;
            }
            this.ParseSolution(buildCakeTask, initOptions.SolutionFilePath);
            return templatePlan;
        }

        private BuildCakeTask ParseSolution(BuildCakeTask buildCakeTask, FilePath slnFilePath)
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
                var scriptTasks = scriptTaskFactory.CreateTaskTemplate(project);
                buildCakeTask.AddScriptTasks(scriptTasks);
            }
            LogHelper.DecreaseIndent();
            return buildCakeTask;
        }

        private TemplatePlan CreateBaseTemplatePlan(BuildCakeTask buildCakeTask, bool shouldOverwrite)
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan.AddStep(new CopyFileStep(templateFileProvider, "build\\build.ps1", "build\\build.ps1", shouldOverwrite));
            templatePlan.AddStep(buildCakeTask);
            return templatePlan;
        }

    }
}
