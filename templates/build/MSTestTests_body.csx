var task = CurrentTask as MsTestTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory()).FullPath;

$@"Task(""{task.Name}"")
    .IsDependentOn(""Build"")
    .Does(() =>
{{
    MSTest($""{solutionDir}/**/bin/{{configuration}}/*.Tests.dll"", new MSTestSettings {{
        
    }});
}});
"