namespace DotNetCake
{
    public class BuildGenerator
    {

        private TemplateFileProvider templateFileProvider;

        public BuildGenerator(TemplateFileProvider templateFileProvider)
        {
            this.templateFileProvider = templateFileProvider;
        }

        public void Generate()
        {
            templateFileProvider.WriteTemplateFile("build/build.ps1");
            templateFileProvider.WriteTemplateFile("build/build.cake");
        }
    }
}