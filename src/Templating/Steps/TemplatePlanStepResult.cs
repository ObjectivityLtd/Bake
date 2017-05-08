using Cake.Core.IO;
using System.Collections.Generic;

namespace Cake.CD.Templating.Steps
{
    public class TemplatePlanStepResult
    {

        public List<FilePath> FilesAdded { get; private set; }

        public TemplatePlanStepResult()
        {
            this.FilesAdded = new List<FilePath>();
        }

        public TemplatePlanStepResult(FilePath fileAdded) : this(new List<FilePath>() { fileAdded })
        {
            
        }

        public TemplatePlanStepResult(List<FilePath> filesAdded)
        {
            this.FilesAdded = filesAdded;
        }
        
    }
}
