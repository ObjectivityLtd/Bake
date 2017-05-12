var task = CurrentTask as NpmTestsTask;

var projectPath = BuildScriptPath.GetRelativePath(task.ProjectDir).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Runs npm tests for {task.NpmProjectName}"")
    .IsDependentOn(""BuildFrontend"")
    .Does(() =>
    {{
        var srcDir = ""{projectPath}"";
        
        NpmRunScript(new NpmRunScriptSettings {{
            LogLevel = NpmLogLevel.Info,
            WorkingDirectory = srcDir,
            ScriptName = ""test""
        }};

    }});
"