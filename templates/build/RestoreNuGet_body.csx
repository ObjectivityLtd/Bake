var task = CurrentTask as RestoreNuGetTask;

var solutionPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Does(() =>
    {{
        NuGetRestore(""{solutionPath}"");
    }});
"