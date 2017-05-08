using Cake.CD.Templating.Steps;
using Cake.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Cake.CD.Templating
{
    public class TemplatePlanResult
    {
        private List<TemplatePlanStepResult> stepResults;

        public TemplatePlanResult(List<TemplatePlanStepResult> stepResults)
        {
            this.stepResults = stepResults;
        }

        public List<FilePath> GetAddedFiles()
        {
            return stepResults.SelectMany(stepResult => stepResult.FilesAdded).ToList();
        }
    }
}
