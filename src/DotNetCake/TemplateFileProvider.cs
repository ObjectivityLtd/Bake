using System;
using System.IO;
using System.Reflection;

public class TemplateFileProvider
{

    public void WriteTemplateFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            Console.WriteLine("File '" + filePath + "' already exists - skipping.");
            return;
        }
        var srcPath = GetPathToTemplateFile(filePath);
        var dstDir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dstDir))
        {
            Console.WriteLine("Creating directory '" + dstDir + "'.");
            Directory.CreateDirectory(dstDir);
        }
        Console.WriteLine("Creating file '" + filePath + "'.");
        File.Copy(srcPath, filePath);
    }

    private string GetPathToTemplateFile(string filePath)
    {
        var assemblyLocation = Path.GetDirectoryName(typeof(TemplateFileProvider).GetTypeInfo().Assembly.Location);
        var srcLocation = "templates/" + filePath;
        for (int i = 0; i < 3; i++)
        {
            var fullSrcLocation = Path.GetFullPath(Path.Combine(assemblyLocation, srcLocation));
            if (File.Exists(fullSrcLocation))
            {
                return fullSrcLocation;
            }
            srcLocation = "../" + srcLocation;
        }
        throw new InvalidOperationException("Cannot find template file 'templates/" + filePath + "' at '" + assemblyLocation + "' or its parent directories.");
    }
}