public class BuildGenerator 
{

    private TemplateFileProvider templateFileProvider = new TemplateFileProvider();

    public void Generate()
    {
        templateFileProvider.WriteTemplateFile("build/build.ps1");
        templateFileProvider.WriteTemplateFile("build/build.cake");
    }
}