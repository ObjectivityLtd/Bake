var task = CurrentTask as GulpTask;

var projectPath = BuildScriptPath.GetRelativePath(task.ProjectDir).FullPath;
var projectName = task.ProjectDir.GetDirectoryName();

$@"
Task(""{task.Name}"")
    .Description(""Builds gulp package {task.NpmProjectName}"")
    .Does(() =>
    {{
        var projectName = ""{task.NpmProjectName}"";
        var srcDir = Directory(""{projectPath}"");
        var outDir = outputDir + Directory(projectName);
        var outputFile = outDir + File(projectName + "".zip"");

        var npmSettings = new NpmInstallSettings
        {{
            LogLevel = NpmLogLevel.Warn,
            WorkingDirectory = srcDir
        }};

        NpmInstall(npmSettings);

        //var powershellSettings = new PowershellSettings()
        //{{
        //   WorkingDirectory = srcDir
        //}};

        // StartPowershellScript(""jspm install"", powershellSettings);

        StartProcess(""cmd"", new ProcessSettings
        {{
            Arguments = ""/c \""gulp build\"""",
            WorkingDirectory = srcDir
        }});

        StartProcess(""cmd"", new ProcessSettings
        {{
            Arguments = ""/c \""gulp export\"""",
            WorkingDirectory = srcDir
        }});

        var exportDir = srcDir + Directory(""export"");
        CreateDirectory(outDir);
        Zip(exportDir, outputFile);
    }});
"