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

        public TemplatePlanResult Generate(FilePath slnFilePath)
        {
            TemplatePlan templatePlan = slnFilePath != null ? templatePlanFactory.CreateTemplatePlanFromSln(slnFilePath) : 
                templatePlanFactory.CreateDefaultTemplatePlan();

            return templatePlan.Execute();
        }

    }
}