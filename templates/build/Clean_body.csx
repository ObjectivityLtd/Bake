var task = CurrentTask as CleanTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory()).FullPath;

string result = $@"
Task(""{task.Name}"")
    .Description(""Cleans output and intermediate folders"")
    .Does(() =>
    {{
        var solutionDir = ""{solutionDir}"";

        CleanDirectories(outputDir);
        CleanDirectories(solutionDir + $""/**/obj/{configuration}"");
        CleanDirectories(solutionDir + $""/**/bin/{configuration}"");
    }});

";

result