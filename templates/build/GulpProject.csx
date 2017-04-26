Task("BuildFrontend")
    .Description("Builds Frontend package")
    .Does(() =>
    {
        Npm.WithLogLevel(NpmLogLevel.Warn).FromPath(sourceDir + "\\wwwroot").Install();
        StartPowershellScript("Push-Location -Path \"..\\..\\src\\wwwroot\" -StackName \"build\"; jspm install; Pop-Location -StackName \"build\";");

        var projectDir = sourceDir + "\\wwwroot";

        StartProcess("cmd", new ProcessSettings
        {
            Arguments = "/c \"gulp build\"",
            WorkingDirectory = projectDir
        });

        StartProcess("cmd", new ProcessSettings
        {
            Arguments = "/c \"gulp export\"",
            WorkingDirectory = projectDir
        });

        var exportDir = sourceDir + "\\wwwroot\\export";
        var frontendOutputDir = outputDir + "\\Frontend";
        var frontendOutputZip = frontendOutputDir + "\\Frontend.zip";

        CreateDirectory(frontendOutputDir);

        Zip(exportDir, frontendOutputZip);
    });