var task = CurrentTask as RestoreNuGetTask;

var solutionPath = CakeScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Does(() =>
    {{
        NuGetRestore(""{solutionPath}"");
    }});
"