using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cake.CD.Templating
{
    public class TemplateFileProvider
    {

        public void WriteTemplateFiles(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                this.WriteTemplateFile(filePath);
            }
        }

        public void WriteTemplateFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Log.Information("File {FilePath} already exists - skipping.", filePath);
                return;
            }
            var srcPath = GetPathToTemplateFile(filePath);
            var dstDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dstDir))
            {
                Log.Information("Creating directory {Dir}.", dstDir);
                Directory.CreateDirectory(dstDir);
            }
            Log.Information("Creating file {File}.", filePath);
            File.Copy(srcPath, filePath);
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