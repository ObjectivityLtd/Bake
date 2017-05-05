using System.Collections.Generic;

namespace Cake.CD.Templating
{
    public class TemplatePlanStepResult
    {

        public List<string> FilesAdded { get; private set; }

        public TemplatePlanStepResult()
        {
            this.FilesAdded = new List<string>();
        }

        public TemplatePlanStepResult(string fileAdded) : this(new List<string>() { fileAdded })
        {
            
        }

        public TemplatePlanStepResult(List<string> filesAdded)
        {
            this.FilesAdded = filesAdded;
        }
        
    }
}
