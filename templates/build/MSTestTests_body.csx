var task = CurrentTask as MsTestTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory());
var solutionName = SolutionFilePath.GetFilenameWithoutExtension();

$@"Task(""{task.Name}"")
    .Description(""Runs mstest tests for {solutionName}"")
    .IsDependentOn(""BuildBackend"")
    .Does(() =>
{{
    MSTest($""{solutionDir.FullPath}/**/bin/{{configuration}}/*.Tests.dll"", new MSTestSettings {{
        
    }});
}});
"