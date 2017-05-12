var task = CurrentTask as PsciTask;

var psciDeployScriptPath = BuildScriptPath.GetRelativePath(task.PsciScriptsBaseDir).CombineWithFilePath("deploy.ps1");
var psciScriptsDeployDir = BuildScriptPath.GetRelativePath(task.PsciScriptsDeployDir).FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Package PSCI deploy scripts and PSCI itself"")
    .Does(() =>
    {{
        var psciDeployScriptPath = ""{psciDeployScriptPath}"";
        var psciDeployDir = ""{psciScriptsDeployDir}"";
        var outDir = outputDir + Directory(""DeployScripts"");

        CreateDirectory(outDir);
        CopyFileToDirectory(psciDeployScriptPath, outDir);
        CopyDirectory(psciScriptsDeployDir, outDir + Directory(""deploy"");

        NuGetInstall(""PSCI"", new NuGetInstallSettings {{
            ExcludeVersion = true,
            OutputDirectory = outputDir + Directory(""PSCI"")
        }});
    }});
"