using System.Collections.Generic;

namespace Cake.CD.Templating
{
    public class TemplatePlan
    {

        public List<ICakeTaskTemplate> TaskTemplates { get; private set; }

        public TemplatePlan()
        {
            this.TaskTemplates = new List<ICakeTaskTemplate>();
        }

        public void AddTaskTemplate(ICakeTaskTemplate taskTemplate)
        {
            if (taskTemplate != null)
            {
                this.TaskTemplates.Add(taskTemplate);
            }
        }

    }
}
