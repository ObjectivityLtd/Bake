using Cake.Core.IO;
using System.Collections.Generic;
using System.Linq;
using Bake.API.EntryScript;

namespace Bake.Templating.Plan
{
    public class TemplatePlanResult
    {
        private IEnumerable<TemplatePlanStepResult> stepResults;

        public TemplatePlanResult(IEnumerable<TemplatePlanStepResult> stepResults)
        {
            this.stepResults = stepResults;
        }

        public IEnumerable<FilePath> GetAddedFiles()
        {
            return stepResults.SelectMany(stepResult => stepResult.FilesAdded).ToList();
        }
    }
}
