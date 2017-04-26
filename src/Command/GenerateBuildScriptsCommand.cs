using Cake.CD.Templating;
using System.Collections.Generic;

namespace Cake.CD.Command
{
    public class GenerateBuildScriptsCommand
    {

        private TemplateFileProvider templateFileProvider;

        public GenerateBuildScriptsCommand(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
        }

        public List<string> Generate()
        {
            var filePaths = new List<string>() { "build\\build.ps1", "build\\build.cake" };
            templateFileProvider.WriteTemplateFiles(filePaths);
            return filePaths;
        }
    }
}