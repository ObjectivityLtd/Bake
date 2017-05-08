using System;
using System.IO;
using System.Reflection;

namespace Cake.CD.Templating
{
    public class TemplateFileProvider
    {

        public string GetOptionalFileContents(string filePath)
        {
            var resolvedFilePath = this.GetPathToTemplateFile(filePath, true);
            return resolvedFilePath == null ? null : File.ReadAllText(resolvedFilePath);
        }

        public string GetMandatoryFileContents(string filePath)
        {
            var resolvedFilePath = this.GetPathToTemplateFile(filePath, false);
            return File.ReadAllText(resolvedFilePath);
        }

        private string GetPathToTemplateFile(string filePath, bool optional)
        {
            var assemblyLocation = Path.GetDirectoryName(typeof(TemplateFileProvider).GetTypeInfo().Assembly.Location);
            var srcLocation = "templates\\" + filePath;
            for (int i = 0; i < 5; i++)
            {
                var fullSrcLocation = Path.GetFullPath(Path.Combine(assemblyLocation, srcLocation));
                if (File.Exists(fullSrcLocation))
                {
                    return fullSrcLocation;
                }
                srcLocation = "../" + srcLocation;
            }
            if (!optional)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot find template file 'templates/{0}' at '{1}' or its parent directories.", filePath, assemblyLocation));
            }
            return null;
        }
    }
}