using Cake.CD.Templating;

namespace Cake.CD.Command
{
    public class CommandRunner
    {

        private TemplateFileProvider templateFileProvider;

        private GenerateBuildScriptsCommand generateBuildScriptsCommand;

        private GenerateDeployScriptsCommand generateDeployScriptsCommand;

        public CommandRunner(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
            this.generateBuildScriptsCommand = new GenerateBuildScriptsCommand(templateFileProvider);
            this.generateDeployScriptsCommand = new GenerateDeployScriptsCommand(templateFileProvider);

        }

        public void GenerateBuildScripts()
        {
            this.generateBuildScriptsCommand.Generate();
        }

        public void GenerateDeployScripts()
        {
            this.generateDeployScriptsCommand.Generate();
        }
    }
}
