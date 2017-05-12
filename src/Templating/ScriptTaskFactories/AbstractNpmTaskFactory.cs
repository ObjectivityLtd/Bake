using Cake.Core.IO;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public class AbstractNpmTaskFactory
    {
        protected bool IsPackageJsonPresent(DirectoryPath projectDir)
        {
            return System.IO.File.Exists(GetPackageJsonPath(projectDir).FullPath);
        }

        protected bool IsNpmTestScriptPresent(DirectoryPath projectDir)
        {
            var packageJsonPath = GetPackageJsonPath(projectDir);
            var json = readJson(packageJsonPath);
            return (json["scripts"]?["test"] != null);
        }

        protected string GetNpmProjectName(DirectoryPath projectDir)
        {
            var packageJsonPath = GetPackageJsonPath(projectDir);
            var json = readJson(packageJsonPath);
            return json["name"]?.ToString();
        }

        private FilePath GetPackageJsonPath(DirectoryPath projectDir)
        {
            return projectDir.CombineWithFilePath("package.json");
        }

        private JObject readJson(FilePath jsonFilePath)
        {
            Log.Debug("Parsing {File}.", jsonFilePath.FullPath);
            return JObject.Parse(System.IO.File.ReadAllText(jsonFilePath.FullPath));
        }
    }
}
