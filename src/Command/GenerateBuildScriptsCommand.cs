using Cake.CD.Templating;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;
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

        public List<string> Generate(string slnFilePath)
        {
            TemplatePlan templatePlan = !String.IsNullOrWhiteSpace(slnFilePath) ? templatePlanFactory.CreateTemplatePlanFromSln(slnFilePath) : 
                templatePlanFactory.CreateDefaultTemplatePlan();

            var filePaths = new List<string>() { "build\\build.ps1", "build\\build.cake" };
            templateFileProvider.WriteTemplateFiles(filePaths);
            return filePaths;
        }

    }
}