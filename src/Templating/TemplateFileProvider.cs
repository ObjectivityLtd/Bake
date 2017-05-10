using Cake.Core.IO;
using System;
using System.IO;
using System.Reflection;

namespace Cake.CD.Templating
{
    public class TemplateFileProvider
    {

        public string GetFileContents(FilePath filePath, bool optional)
        {
            var resolvedFilePath = this.GetPathToTemplateFile(filePath, optional);
            return resolvedFilePath == null ? null : File.ReadAllText(resolvedFilePath);
        }

        private string GetPathToTemplateFile(FilePath filePath, bool optional)
        {
            var assemblyLocation = new FilePath((typeof(TemplateFileProvider).GetTypeInfo().Assembly.Location)).GetDirectory();
            var srcLocation = new FilePath("templates/" + filePath);
            for (int i = 0; i < 5; i++)
            {
                var fullSrcLocation = assemblyLocation.CombineWithFilePath(srcLocation).FullPath;
                if (File.Exists(fullSrcLocation))
                {
                    return fullSrcLocation;
                }
                srcLocation = "../" + srcLocation;
            }
            if (!optional)
            {
                throw new InvalidOperationException(
                    $"Cannot find template file 'templates/{filePath}' at '{assemblyLocation}' or its parent directories.");
            }
            return null;
        }
    }
}