using Cake.CD.Templating;

namespace Cake.CD.Command
{
    public class GenerateDeployScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        public GenerateDeployScriptsCommand(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
        }

        public void Generate()
        {
            templateFileProvider.WriteTemplateFile("deploy/deploy.ps1");
            templateFileProvider.WriteTemplateFile("deploy/deploy.cake");
        }
    }
}