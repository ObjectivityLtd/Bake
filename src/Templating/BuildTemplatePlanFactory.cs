using Cake.CD.Command;
using Cake.CD.MsBuild;
using Cake.CD.Templating.Steps;
using Cake.CD.Templating.Steps.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core.IO;
using Serilog;

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
            Log.Information("Creating templates basing on solution file {SlnFilePath}.", initOptions.SolutionFilePath);
            
            this.ParseSolution(buildCakeTask, initOptions.SolutionFilePath);
            return templatePlan;
        }

        private BuildCakeTask ParseSolution(BuildCakeTask buildCakeTask, FilePath slnFilePath)
        {
            Log.Information("Parsing solution {SlnFile}.", slnFilePath);
            var solutionParserResult = solutionParser.Parse(slnFilePath);
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                IScriptTask scriptTask = scriptTaskFactory.CreateTaskTemplate(project);
                if (scriptTask != null)
                {
                    buildCakeTask.AddScriptTask(scriptTask);
                }
            }
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
