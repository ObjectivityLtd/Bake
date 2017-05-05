using System;
using System.IO;
using System.Reflection;

namespace Cake.CD.Templating
{
    public class TemplateFileProvider
    {

        public string GetFileContents(string filePath)
        {
            var resolvedFilePath = this.GetPathToTemplateFile(filePath);
            return File.ReadAllText(resolvedFilePath);
        }

        private string GetPathToTemplateFile(string filePath)
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(TemplateFileProvider).GetTypeInfo().Assembly.Location);
            var srcLocation = "templates/" + filePath;
            for (int i = 0; i < 5; i++)
            {
                var fullSrcLocation = Path.GetFullPath(Path.Combine(assemblyLocation, srcLocation));
                if (File.Exists(fullSrcLocation))
                {
                    return fullSrcLocation;
                }
                srcLocation = "../" + srcLocation;
            }
            throw new InvalidOperationException(
                String.Format("Cannot find template file 'templates/{0}' at '{1}' or its parent directories.", filePath, assemblyLocation));
        }
    }
}