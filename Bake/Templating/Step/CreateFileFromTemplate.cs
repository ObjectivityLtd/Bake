using Cake.Core.IO;
using System.IO;
using Bake.API.EntryScript;
using Bake.API.Logging;

namespace Bake.Templating.Step
{
    public class CreateFileFromTemplate : ITemplatePlanStep
    {
        private readonly string contents;

        private readonly FilePath dstPath;

        private readonly bool overwrite;

        public CreateFileFromTemplate(TemplateFileProvider templateFileProvider, FilePath srcPath, FilePath dstPath, bool overwrite)
        {
            this.contents = templateFileProvider.GetFileContents(srcPath, true);
            this.dstPath = dstPath.MakeAbsolute(Directory.GetCurrentDirectory());
            this.overwrite = overwrite;
        }

        public TemplatePlanStepResult Execute()
        {
            var fileExists = File.Exists(dstPath.FullPath);
            if (!this.overwrite && fileExists)
            {
                Log.Info("File {FilePath} already exists - skipping.", dstPath.FullPath);
                return new TemplatePlanStepResult();
            }
            var dstDir = dstPath.GetDirectory();
            if (!Directory.Exists(dstDir.FullPath))
            {
                Log.Info("Creating directory {Dir}.", dstDir.FullPath);
                Directory.CreateDirectory(dstDir.FullPath);
            }
            Log.Info("{Creating} file {File}.", fileExists ? "Overwriting" : "Creating", dstPath.FullPath);
            File.WriteAllText(dstPath.FullPath, contents);
            return new TemplatePlanStepResult(dstPath);
        }

    }
}
