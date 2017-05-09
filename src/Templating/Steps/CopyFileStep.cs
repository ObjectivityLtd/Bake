using Cake.Core.IO;
using Serilog;
using System.IO;

namespace Cake.CD.Templating.Steps
{
    public class CopyFileStep : ITemplatePlanStep
    {
        private readonly string contents;

        private readonly FilePath dstPath;

        private readonly bool overwrite;

        public CopyFileStep(TemplateFileProvider templateFileProvider, FilePath srcPath, FilePath dstPath, bool overwrite)
        {
            this.contents = templateFileProvider.GetMandatoryFileContents(srcPath);
            this.dstPath = dstPath.MakeAbsolute(Directory.GetCurrentDirectory());
            this.overwrite = overwrite;
        }

        public TemplatePlanStepResult Execute()
        {
            var fileExists = File.Exists(dstPath.FullPath);
            if (!this.overwrite && fileExists)
            {
                Log.Information("File {FilePath} already exists - skipping.", dstPath);
                return new TemplatePlanStepResult();
            }
            var dstDir = dstPath.GetDirectory();
            if (!Directory.Exists(dstDir.FullPath))
            {
                Log.Information("Creating directory {Dir}.", dstDir.FullPath);
                Directory.CreateDirectory(dstDir.FullPath);
            }
            if (fileExists)
            {
                Log.Information("Overwriting file {File}.", dstPath.FullPath);
            } else
            {
                Log.Information("Creating file {File}.", dstPath.FullPath);
            }
            File.WriteAllText(dstPath.FullPath, contents);
            return new TemplatePlanStepResult(dstPath);
        }

    }
}
