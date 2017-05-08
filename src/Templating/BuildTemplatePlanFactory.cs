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

        private BuildCake buildCake;

        public BuildTemplatePlanFactory(SolutionParser solutionParser, ProjectParser projectParser, ScriptTaskFactory scriptTaskFactory, 
            TemplateFileProvider templateFileProvider, BuildCake buildCake)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.scriptTaskFactory = scriptTaskFactory;
            this.templateFileProvider = templateFileProvider;
            this.buildCake = buildCake;
        }

        public TemplatePlan CreateTemplatePlanFromSln(FilePath slnFilePath)
        {
            Log.Information("Creating templates basing on solution file {SlnFilePath}.", slnFilePath);
            TemplatePlan templatePlan = this.CreateBaseTemplatePlan();
            this.ParseSolution(slnFilePath);
            return templatePlan;
        }

        public TemplatePlan CreateDefaultTemplatePlan()
        {
            Log.Warning("No solution file provided - creating default templates.");
            TemplatePlan templatePlan = this.CreateBaseTemplatePlan();
            return templatePlan;
        }

        private BuildCake ParseSolution(FilePath slnFilePath)
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
                    buildCake.AddScriptTask(scriptTask);
                }
            }
            return buildCake;
        }

        private TemplatePlan CreateBaseTemplatePlan()
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan.AddStep(new CopyFileStep(templateFileProvider, "build\\build.ps1", "build\\build.ps1"));
            templatePlan.AddStep(buildCake);
            return templatePlan;
        }

    }
}
