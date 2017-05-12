using Cake.CD.Templating.Steps;
using Serilog;
using System.Collections.Generic;
using Cake.CD.Logging;
using Serilog.Context;

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
            var i = 1;
            LogHelper.LogHeader("Creating build scripts");
            foreach (var step in steps)
            {
                Log.Information("Executing step {Step} ({i}/{total}).", step, i, steps.Count);
                LogHelper.IncreaseIndent();
                stepResults.Add(step.Execute());
                LogHelper.DecreaseIndent();
                i++;
            }
            return new TemplatePlanResult(stepResults);
        }

    }
}
