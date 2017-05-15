using Cake.CD.Templating;
using System.Collections.Generic;
using Cake.CD.Templating.Plan;

namespace Cake.CD.Command
{
    public class GenerateDeployScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        public GenerateDeployScriptsCommand(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
        }

        public TemplatePlanResult Generate(InitOptions initOptions)
        {
            var filePaths = new List<string>() { "deploy\\deploy.ps1", "deploy\\deploy.cake" };
            // TODO
            return null;
        }
    }
}