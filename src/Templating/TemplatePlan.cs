using Cake.CD.Templating.Steps;
using Serilog;
using System.Collections.Generic;

namespace Cake.CD.Templating
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
            foreach (var step in steps)
            {
                Log.Information("Executing step {Step}.", step);
                var stepResult = step.Execute();
                stepResults.Add(stepResult);
            }
            return new TemplatePlanResult(stepResults);
        }

    }
}
