using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace DotNetCake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Cake.CD");
            WriteBuildTemplates();
            
        }

        private static void WriteBuildTemplates()
        {
            WriteTemplateFile("build/build.ps1");
            WriteTemplateFile("build/build.cake");
        }

        private static void WriteTemplateFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine("File '" + filePath + "' already exists - skipping.");
                return;
            }
            var assemblyLocation = Path.GetDirectoryName(typeof(Program).GetTypeInfo().Assembly.Location);
            var srcLocation = Path.Combine(assemblyLocation, "templates", filePath);
            var dstDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dstDir))
            {
                Console.WriteLine("Creating directory '" + dstDir + "'.");
                Directory.CreateDirectory(dstDir);
            }
            Console.WriteLine("Creating file '" + filePath + "'.");
            File.Copy(srcLocation, filePath);
        }
    }
}

