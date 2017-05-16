using Bake.API.EntryScript;
using Bake.Templating.Plan;

namespace Bake.Command
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