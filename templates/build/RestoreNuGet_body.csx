var task = CurrentTask as RestoreNuGetTask;

var projectPath = BuildScriptPath.GetRelativePath(task.SourceFile).FullPath;

$@"
Task(""{task.Name}"")
    .Does(() =>
    {{
        NuGetRestore(""{projectPath}"");
    }});
"