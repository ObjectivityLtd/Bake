namespace Bake.Command
{
    public class CommandRunner
    {

        public GenerateBuildScriptsCommand GenerateBuildScriptsCommand { get; }

        public GenerateDeployScriptsCommand GenerateDeployScriptsCommand { get; }

        public UpdateVisualStudioSlnCommand UpdateVisualStudioSlnCommand { get; }

        public InitCommand InitCommand { get; }

        public CommandRunner(GenerateBuildScriptsCommand generateBuildScriptsCommand, GenerateDeployScriptsCommand generateDeployScriptsCommand,
            UpdateVisualStudioSlnCommand updateVisualStudioSlnCommand, InitCommand initCommand)
        {
            this.GenerateBuildScriptsCommand = generateBuildScriptsCommand;
            this.GenerateDeployScriptsCommand = generateDeployScriptsCommand;
            this.UpdateVisualStudioSlnCommand = updateVisualStudioSlnCommand;
            this.InitCommand = initCommand;
        }

    }
}
