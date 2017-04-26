using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.CD.Templating
{
    public class MsBuildTaskTemplate : ICakeTaskTemplate
    {

        public enum MsBuildTaskType
        {
            WEB_APPLICATION,
            CONSOLE_APPLICATION
        }

        public MsBuildTaskType TaskType { get; private set; }

        public string SourceFile { get; private set; }

        public string DestinationFile { get; private set; }

        public MsBuildTaskTemplate(MsBuildTaskType TaskType, string SourceFile, string DestinationFile)
        {
            this.TaskType = TaskType;
            this.SourceFile = SourceFile;
            this.DestinationFile = DestinationFile;
        }


    }
}
