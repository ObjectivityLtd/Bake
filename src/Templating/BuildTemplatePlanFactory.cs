using Cake.CD.MsBuild;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;
using System.Linq;

namespace Cake.CD.Templating
{
    public class BuildTemplatePlanFactory
    {

        private SolutionParser solutionParser;

        private ProjectParser projectParser;

        private TaskTemplateFactory taskTemplateFactory;

        public BuildTemplatePlanFactory(SolutionParser solutionParser, ProjectParser projectParser, TaskTemplateFactory taskTemplateFactory)
        {
            this.solutionParser = solutionParser;
            this.projectParser = projectParser;
            this.taskTemplateFactory = taskTemplateFactory;
        }

        public TemplatePlan CreateTemplatePlanFromSln(string slnFilePath)
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan = ParseSolution(slnFilePath, templatePlan);
            return templatePlan;
        }

        public TemplatePlan CreateDefaultTemplatePlan()
        {
            TemplatePlan templatePlan = new TemplatePlan();
            return templatePlan;
        }

        private TemplatePlan ParseSolution(string slnFilePath, TemplatePlan templatePlan)
        {
            Log.Information("Parsing solution {SlnFile}.", slnFilePath);
            var solutionParserResult = solutionParser.Parse(new Core.IO.FilePath(slnFilePath));
            foreach (var project in solutionParserResult.Projects)
            {
                if (project.Type == null || !MsBuildGuids.IsSupportedSlnTypeIdentifier(project.Type))
                {
                    continue;
                }
                ICakeTaskTemplate taskTemplate = taskTemplateFactory.CreateTaskTemplate(project);
                templatePlan.AddTaskTemplate(taskTemplate);

            }
            return templatePlan;
        }
    }
}
