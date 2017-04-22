Task("Deploy")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Default")
    .IsDependentOn("Deploy");

RunTarget(target);