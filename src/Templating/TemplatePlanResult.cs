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

        public List<string> GetAddedFiles()
        {
            return stepResults.SelectMany(stepResult => stepResult.FilesAdded).ToList();
        }
    }
}
