using Cake.CD.MsBuild;
using Cake.CD.Templating.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;

namespace Cake.CD.Templating
{
    public class BuildTemplatePlanFactory
    {

        private SolutionParser solutionParser;

        private ProjectParser projectParser;

        private TaskScriptFactory taskScriptFactory;

        private TemplateFileProvider templateFileProvider;

        private BuildCake buildCake;

        public BuildTemplatePlanFactory(SolutionParser solutionParser, ProjectParser projectParser, TaskScriptFactory taskScriptFactory, 
            TemplateFileProvider templateFileProvider, BuildCake buildCake)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.taskScriptFactory = taskScriptFactory;
            this.templateFileProvider = templateFileProvider;
            this.buildCake = buildCake;
        }

        public TemplatePlan CreateTemplatePlanFromSln(string slnFilePath)
        {
            TemplatePlan templatePlan = this.CreateBaseTemplatePlan();
            this.ParseSolution(slnFilePath);
            return templatePlan;
        }

        public TemplatePlan CreateDefaultTemplatePlan()
        {
            TemplatePlan templatePlan = this.CreateBaseTemplatePlan();
            return templatePlan;
        }

        private BuildCake ParseSolution(string slnFilePath)
        {
            Log.Information("Parsing solution {SlnFile}.", slnFilePath);
            var solutionParserResult = solutionParser.Parse(new Core.IO.FilePath(slnFilePath));
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                IScriptTask taskScript = taskScriptFactory.CreateTaskTemplate(project);
                buildCake.AddTaskScript(taskScript);

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
