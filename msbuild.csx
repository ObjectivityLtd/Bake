Task("BuildBackend")
    .Description("Builds backend soloutution")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var solution = sourceDir + "\\Objectivity.Holiday.Backend.sln";
        var backendOutputZip = outputDir + "\\Backend\\Backend.zip";

        NuGetRestore(solution);

        MSBuild(solution, settings =>
            settings.WithProperty("VisualStudioVersion", "14.0")
                    .WithProperty("DeployTarget", "Package")
                    .WithProperty("DeployOnBuild", "True")
                    .WithProperty("AutoParameterizationWebConfigConnectionStrings", "false")
                    .WithProperty("PackageLocation", backendOutputZip)
                    .SetConfiguration("Release")
                    .SetNodeReuse(false));
    });