using Cake.Core.IO;

namespace Cake.CD.Command
{
    public class InitOptions
    {
        public FilePath SolutionFilePath;

        public bool Overwrite;

        public bool BuildSolution;
    }
}
