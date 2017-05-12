var task = CurrentTask as XUnitTestsTask;

var solutionDir = BuildScriptPath.GetRelativePath(SolutionFilePath.GetDirectory()).FullPath;

$@"Task(""{task.Name}"")
    .IsDependentOn(""Build"")
    .Does(() =>
{{
    XUnit2($""{solutionDir}/**/bin/{{configuration}}/*.Tests.dll"", new XUnit2Settings {{
        
    }});
}});
"