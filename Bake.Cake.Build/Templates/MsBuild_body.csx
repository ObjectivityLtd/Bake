var task = CurrentTask as MsBuildTask;

var projectPaths = task.SourceFiles.Select(file => BuildScriptPath.GetRelativePath(file).FullPath).ToList();
var projectPathsString = string.Join("," + Environment.NewLine + "            ", projectPaths.Select(path => "\"" + path + "\""));
var restoreNuget = SolutionFilePath == null;
var nugetPackagesDirectory = restoreNuget ? null : BuildScriptPath.GetRelativePath(SolutionDir).FullPath;
var description = $"Builds {task.TaskType} package" + (task.ProjectName != "" ? $" {task.ProjectName}" : "");

var result = $@"
Task(""{task.Name}"")
    .Description(""{description}"")
    .Does(() =>
    {{";

if (projectPaths.Count > 1)
{
    result += $@"
    var projectPaths = new List<string> {{ 
        {projectPathsString}
    }};
    foreach (var projectPath in projectPaths) {{";
}
else
{
    result += $@"
        var projectPath = File({projectPathsString});";

    if (task.CreateMsDeployPackage)
    {
        result += $@"
        var outputZip = outputDir + File(""{task.ProjectName}/{task.ProjectName}.zip"");";
    } else
    {
        result += $@"
        var outDir = outputDir + Directory(""{task.ProjectName}"");";
    }
}

if (restoreNuget)
{
    if (nugetPackagesDirectory != null)
    {
        result += $@"
        NuGetRestore(projectPath, new NuGetRestoreSettings {{ PackagesDirectory = ""{nugetPackagesDirectory}"" }});";
    }
    else
    {
        result += $@"
        NuGetRestore(projectPath);";
    }
}

if (task.CreateMsDeployPackage)
{
    result += $@"
        MSBuild(projectPath, defaultMsBuildPackageSettings.WithProperty(""PackageLocation"", outputZip));";
}
else if (task.TaskType != MsBuildTask.MsBuildTaskType.Solution && task.TaskType != MsBuildTask.MsBuildTaskType.UnitTests)
{
    result += $@"
        MSBuild(projectPath, defaultMsBuildCommonSettings.WithTarget(""Build"").WithProperty(""OutDir"", outDir)); ";
}
else
{
    result += $@"
        MSBuild(projectPath, defaultMsBuildCommonSettings.WithTarget(""Build""));";
}

if (projectPaths.Count > 1)
{
    result += $@"
    }}";
}

result += $@"
}});";


return result;