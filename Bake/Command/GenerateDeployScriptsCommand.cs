using Bake.API.EntryScript;
using Bake.Templating;
using Bake.Templating.Plan;
using System.Collections.Generic;

namespace Bake.Command
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