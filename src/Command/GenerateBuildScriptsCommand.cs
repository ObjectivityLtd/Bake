using Cake.CD.Templating;

namespace Cake.CD.Command
{
    public class GenerateBuildScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        public GenerateBuildScriptsCommand(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
        }

        public void Generate()
        {
            templateFileProvider.WriteTemplateFile("build/build.ps1");
            templateFileProvider.WriteTemplateFile("build/build.cake");
        }
    }
}