using Cake.CD.Templating;
using Cake.Core.IO;
using System.Collections.Generic;
using Cake.CD.MsBuild;

namespace Cake.CD.Command
{
    public class UpdateVisualStudioSlnCommand
    {
        private SolutionExtender visualStudioSlnHandler;

        public UpdateVisualStudioSlnCommand(SolutionExtender visualStudioSlnHandler)
        {
            this.visualStudioSlnHandler = visualStudioSlnHandler;
        }

        public void AddSolutionFolderToSlnFile(string slnFilePath, string solutionFolderName, string solutionFolderPath, List<string> filePaths)
        {
            this.visualStudioSlnHandler.AddSolutionFolderToSlnFile(slnFilePath, solutionFolderName, solutionFolderPath, filePaths);
        }
    }
}
