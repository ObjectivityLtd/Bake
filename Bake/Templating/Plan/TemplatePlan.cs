using Bake.API.EntryScript;
using System.Collections.Generic;
using Bake.API.Logging;

namespace Bake.Templating.Plan
{
    public class TemplatePlan
    {
        private List<ITemplatePlanStep> steps;

        public TemplatePlan()
        {
            this.steps = new List<ITemplatePlanStep>();
        }

        public TemplatePlan AddStep(ITemplatePlanStep step)
        {
            this.steps.Add(step);
            return this;
        }

        public TemplatePlanResult Execute()
        {
            var stepResults = new List<TemplatePlanStepResult>();
            var i = 1;
            Log.Header("Creating build scripts");
            foreach (var step in steps)
            {
                Log.Info("Executing step {Step} ({i}/{total}).", step, i, steps.Count);
                Log.IncreaseIndent();
                stepResults.Add(step.Execute());
                Log.DecreaseIndent();
                i++;
            }
            return new TemplatePlanResult(stepResults);
        }

    }
}
