using Cake.CD.Templating;
using System.Collections.Generic;

namespace Cake.CD.Command
{
    public class UpdateVisualStudioSlnCommand
    {
        private VisualStudioSlnHandler visualStudioSlnHandler;

        public UpdateVisualStudioSlnCommand(VisualStudioSlnHandler visualStudioSlnHandler)
        {
            this.visualStudioSlnHandler = visualStudioSlnHandler;
        }

        public void AddSolutionFolderToSlnFile(string slnFilePath, string solutionFolderName, string solutionFolderPath, List<string> filePaths)
        {
            this.visualStudioSlnHandler.AddSolutionFolderToSlnFile(slnFilePath, solutionFolderName, solutionFolderPath, filePaths);
        }
    }
}
