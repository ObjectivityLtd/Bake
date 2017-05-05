using Serilog;
using System.IO;

namespace Cake.CD.Templating
{
    public class CopyFileStep : ITemplatePlanStep
    {
        private readonly string contents;

        private readonly string dstPath;

        public CopyFileStep(TemplateFileProvider templateFileProvider, string srcPath, string dstPath)
        {
            this.contents = templateFileProvider.GetFileContents(srcPath);
            this.dstPath = dstPath;
        }

        public TemplatePlanStepResult Execute()
        {
            if (File.Exists(dstPath))
            {
                Log.Information("File {FilePath} already exists - skipping.", dstPath);
                return new TemplatePlanStepResult();
            }
            var dstDir = Path.GetDirectoryName(dstPath);
            if (!Directory.Exists(dstDir))
            {
                Log.Information("Creating directory {Dir}.", dstDir);
                Directory.CreateDirectory(dstDir);
            }
            Log.Information("Creating file {File}.", dstPath);
            File.WriteAllText(dstPath, contents);
            return new TemplatePlanStepResult(dstPath);
        }

    }
}
