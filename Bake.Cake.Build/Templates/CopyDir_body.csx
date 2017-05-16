var task = CurrentTask as CopyDirTask;

var sourceDir = BuildScriptPath.GetRelativePath(task.SourceDir);
var packageName = sourceDir.GetDirectoryName().FullPath;

$@"
Task(""{task.Name}"")
    .Description(""Copies package {packageName}"")
    .Does(() =>
    {{
        var srcDir = Directory(""{sourceDir.FullPath}"");
        var dstDir = outputDir + Directory(""{packageName}"");

        CopyDirectory(srcDir, dstDir);
    }});
"