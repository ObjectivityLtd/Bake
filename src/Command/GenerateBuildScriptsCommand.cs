using Cake.CD.Templating.Plan;

namespace Cake.CD.Command
{
    public class GenerateBuildScriptsCommand
    {
        private BuildTemplatePlanFactory templatePlanFactory;

        public GenerateBuildScriptsCommand(BuildTemplatePlanFactory templatePlanFactory)
        {
            this.templatePlanFactory = templatePlanFactory;
        }

        public TemplatePlanResult Generate(InitOptions initOptions)
        {
            var templatePlan = templatePlanFactory.CreateTemplatePlan(initOptions);
            return templatePlan.Execute();
        }

    }
}