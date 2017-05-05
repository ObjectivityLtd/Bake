using Cake.CD.Templating;
using Cake.CD.Templating.Build;
using System;
using System.Collections.Generic;

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

        public TemplatePlanResult Generate(string slnFilePath)
        {
            TemplatePlan templatePlan = !String.IsNullOrWhiteSpace(slnFilePath) ? templatePlanFactory.CreateTemplatePlanFromSln(slnFilePath) : 
                templatePlanFactory.CreateDefaultTemplatePlan();

            return templatePlan.Execute();
        }

    }
}