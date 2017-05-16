var task = CurrentTask as RestoreNugetTask;

var solutionFilePath = BuildScriptPath.GetRelativePath(SolutionFilePath).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Restores nuget packages for solution {task.SolutionName}"")
    .Does(() =>
    {{
        NuGetRestore(""{solutionFilePath}"");
    }});
"