Task("BuildMigration")
    .Description("Builds migration package")
    .Does(() =>
    {
        var project = sourceDir + "\\Objectivity.Holiday.DataAccess\\Objectivity.Holiday.DataAccess.csproj";
        var migrationPackagePath = outputDir + "\\HolidayMigration";

        MSBuild(project, settings =>
            settings.WithProperty("OutDir", migrationPackagePath));

        CopyFileToDirectory(sourceDir + "\\packages\\EntityFramework.6.1.3\\tools\\migrate.exe", migrationPackagePath);
        CopyFileToDirectory(sourceDir + "\\packages\\EntityFramework.6.1.3\\lib\\net45\\EntityFramework.dll", migrationPackagePath);
    });