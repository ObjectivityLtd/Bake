var task = CurrentTask as MsBuildTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

var result = $@"
Task(""{task.Name}"")
    .Description(""Builds {task.TaskType} package {task.ProjectName}"")
    .Does(() =>
    {{
        var projectPath = File(""{projectPath}"");";

if (task.CreatePackage)
{
    result += $@"
        var outputZip = outputDir + File(""{task.ProjectName}/{task.ProjectName}.zip"");";
}

if (task.RestoreNuget)
{
    result += $@"
        NuGetRestore(projectPath);";
}

if (task.CreatePackage)
{
    result += $@"
        MSBuild(projectPath, defaultMsBuildPackageSettings.WithProperty(""PackageLocation"", outputZip));";
} else
{
    result += $@"
        MSBuild(projectPath, defaultMsBuildCommonSettings);";
}

result += $@"
    }});";


return result;