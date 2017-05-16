using Cake.Core.IO;
using System.Collections.Generic;

namespace Bake.API.EntryScript
{
    public class TemplatePlanStepResult
    {

        public IEnumerable<FilePath> FilesAdded { get; private set; }

        public TemplatePlanStepResult()
        {
            this.FilesAdded = new List<FilePath>();
        }

        public TemplatePlanStepResult(FilePath fileAdded) : this(new List<FilePath>() { fileAdded })
        {
            
        }

        public TemplatePlanStepResult(IEnumerable<FilePath> filesAdded)
        {
            this.FilesAdded = filesAdded;
        }
        
    }
}
