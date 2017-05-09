using Cake.CD.Templating;
using Cake.Core.IO;
using System;

namespace Cake.CD.Command
{
    public class GenerateBuildScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        private BuildTemplatePlanFactory templatePlanFactory;

        public GenerateBuildScriptsCommand(TemplateFileProvider templateFileProvider, BuildTemplatePlanFactory templatePlanFactory)
        {
            this.templateFileProvider = templateFileProvider;
            this.templatePlanFactory = templatePlanFactory;
        }

        public TemplatePlanResult Generate(InitOptions initOptions)
        {
            var templatePlan = templatePlanFactory.CreateTemplatePlan(initOptions);
            return templatePlan.Execute();
        }

    }
}