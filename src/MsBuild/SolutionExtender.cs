using Cake.Common.Solution;
using Cake.Core.IO;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cake.CD.Templating
{
    // based on Cake.Common.Solution.SolutionParser
    public class SolutionExtender
    {

        public void AddSolutionFolderToSlnFile(string slnFilePath, string solutionFolderName, string solutionFolderPath, List<string> filePaths)
        {
            var lines = ReadSolutionFile(slnFilePath);
            var newLines = EnsureSolutionContainsFolderWithFiles(lines, slnFilePath, solutionFolderName, solutionFolderPath, filePaths);

            if (newLines == lines)
            {
                Log.Information("Files already present in the solution file - skipping.");
                return;
            }

            Log.Information("Saving solution file {SlnFile}", slnFilePath);
            File.WriteAllLines(slnFilePath, newLines, Encoding.UTF8);
        }

        private List<string> ReadSolutionFile(string slnFilePath)
        {
            if (!File.Exists(slnFilePath))
            {
                throw new InvalidOperationException(String.Format("Solution file '{0}' does not exist.", System.IO.Path.GetFullPath(slnFilePath)));
            }
            Log.Information("Parsing solution file {SlnFile}", slnFilePath);
            return File.ReadAllLines(slnFilePath, Encoding.UTF8).ToList();
        }  

        private List<string> EnsureSolutionContainsFolderWithFiles(List<string> lines, string slnFilePath, string solutionFolderName, string solutionFolderPath, List<string> filePaths)
        {
            int lastProjectLine = -1;
            int i = -1;
            var slnDirectory = new DirectoryPath(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(slnFilePath)));
            foreach (var line in lines)
            {
                i++;
                if (line.StartsWith("Project(\"{"))
                {
                    var project = ParseSolutionProjectLine(slnDirectory.FullPath, line);
                    if (StringComparer.OrdinalIgnoreCase.Equals(project.Type, SolutionFolder.TypeIdentifier) && 
                        StringComparer.OrdinalIgnoreCase.Equals(solutionFolderName, project.Name))
                    {
                        var currentSolutionFolderPath = slnDirectory.GetRelativePath(project.Path).FullPath;
                        this.AssertProjectPathsAreEqual(slnFilePath, solutionFolderName, solutionFolderPath, currentSolutionFolderPath);
                        return this.EnsureProjectSectionContainsFiles(lines, i + 2, filePaths);
                    }
                }
                else if (line.StartsWith("EndProject"))
                {
                    lastProjectLine = i;
                }
            }
            return this.AddNewProjectSection(lines, lastProjectLine, solutionFolderName, solutionFolderPath, filePaths);
        }

        private List<string> AddNewProjectSection(List<string> lines, int lastProjectLine, string solutionFolderName, string solutionFolderPath, List<string> filePaths)
        {
            var newLines = new List<string>();
            Log.Information("Adding solution folder {SolutionFolder}.", solutionFolderName);
            newLines.Add(String.Format("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", 
                SolutionFolder.TypeIdentifier, solutionFolderName, solutionFolderPath, Guid.NewGuid().ToString().ToUpper()));
            newLines.Add("\tProjectSection(SolutionItems) = preProject");
            newLines = this.EnsureProjectSectionContainsFiles(newLines, 2, filePaths);
            newLines.Add("\tEndProjectSection");
            newLines.Add("EndProject");

            var result = new List<string>(lines);
            result.InsertRange(lastProjectLine + 1, newLines);
            return result;

        }

        private List<string> EnsureProjectSectionContainsFiles(List<string> lines, int startLine, List<string> filePaths)
        {
            var filePathsToAdd = new List<string>(filePaths);
            int endProjectIndex = startLine;
            int i = startLine;
            foreach (var line in lines.Skip(startLine))
            {
                if (line.TrimStart() == "EndProjectSection")
                {
                    endProjectIndex = i;
                    break;
                }
                var entry = line.Split(new[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                filePathsToAdd.Remove(entry[0].Trim().ToLower());
                i++;
            }
            if (filePathsToAdd.Count == 0)
            {
                return lines;
            }
            foreach (var filePath in filePathsToAdd)
            {
                Log.Information("Adding file {File} to solution folder", filePath);
            }
            filePathsToAdd = filePathsToAdd.Select(path => String.Format("\t\t{0} = {0}", path)).ToList();
            var result = new List<string>(lines);
            result.InsertRange(endProjectIndex, filePathsToAdd);
            return result;
        }

        private void AssertProjectPathsAreEqual(string slnFilePath, string solutionFolderName, string solutionFolderPath, string currentSolutionFolderPath)
        {
            if (!StringComparer.OrdinalIgnoreCase.Equals(solutionFolderPath, currentSolutionFolderPath))
            {
                throw new InvalidOperationException(String.Format(
                    "Solution '{0}' already contains solution folder named '{1}' with path '{2}' (expecting path '{3}')",
                    slnFilePath, solutionFolderName, currentSolutionFolderPath, solutionFolderPath));
            }
        }


        // based on Cake.Common.Solution.SolutionParser
        private SolutionProject ParseSolutionProjectLine(string solutionPath, string line)
        {
            var withinQuotes = false;
            var projectTypeBuilder = new StringBuilder();
            var nameBuilder = new StringBuilder();
            var pathBuilder = new StringBuilder();
            var idBuilder = new StringBuilder();
            var result = new[]
            {
                projectTypeBuilder,
                nameBuilder,
                pathBuilder,
                idBuilder
            };
            var position = 0;
            foreach (var c in line.Skip(8))
            {
                if (c == '"')
                {
                    withinQuotes = !withinQuotes;
                    if (!withinQuotes)
                    {
                        if (position++ >= result.Length)
                        {
                            break;
                        }
                    }
                    continue;
                }
                if (!withinQuotes)
                {
                    continue;
                }
                result[position].Append(c);
            }
            
            return new SolutionProject(
                idBuilder.ToString(),
                nameBuilder.ToString(),
                System.IO.Path.Combine(solutionPath, pathBuilder.ToString()),
                projectTypeBuilder.ToString());
        }

    }
}
