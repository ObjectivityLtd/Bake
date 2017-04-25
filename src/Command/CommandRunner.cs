using Cake.CD.Templating;

namespace Cake.CD.Command
{
    public class CommandRunner
    {

        public GenerateBuildScriptsCommand GenerateBuildScriptsCommand { get; }

        public GenerateDeployScriptsCommand GenerateDeployScriptsCommand { get; }

        public UpdateVisualStudioSlnCommand UpdateVisualStudioSlnCommand { get; }

        public CommandRunner(TemplateFileProvider templateFileProvider)
        {
            var visualStudioSlnHandler = new VisualStudioSlnHandler();
            GenerateBuildScriptsCommand = new GenerateBuildScriptsCommand(templateFileProvider, this);
            GenerateDeployScriptsCommand = new GenerateDeployScriptsCommand(templateFileProvider);
            UpdateVisualStudioSlnCommand = new UpdateVisualStudioSlnCommand(visualStudioSlnHandler);
        }

    }
}
