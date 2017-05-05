using Cake.CD.MsBuild;
using Cake.CD.Templating.Build;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Serilog;

namespace Cake.CD.Templating
{
    public class TaskScriptFactory
    {
        private ScriptTaskEvaluator scriptTaskEvaluator; 

        private ProjectParser projectParser;

        public TaskScriptFactory(ScriptTaskEvaluator scriptTaskEvaluator, ProjectParser projectParser)
        {
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            this.projectParser = projectParser;
        }

        public IScriptTask CreateTaskTemplate(SolutionProject solutionProject)
        {
            Log.Information("Parsing project {ProjectFile}", solutionProject.Path.FullPath, solutionProject.Type);
            ProjectParserResult projectParserResult = projectParser.Parse(solutionProject.Path);
            if (projectParserResult.IsWebApplication(solutionProject.Path.FullPath))
            {
                Log.Information("Project {ProjectFile} is web application - adding msbuild template", solutionProject.Path.FullPath);
                return new MsBuildTask(MsBuildTask.MsBuildTaskType.WEB_APPLICATION, solutionProject.Path.FullPath, "bin\\" + solutionProject.Name + ".zip");
            }
            if (projectParserResult.IsExecutableApplication())
            {
                Log.Information("Project {ProjectFile} is executable application - adding msbuild template", solutionProject.Path.FullPath);
                return new MsBuildTask(MsBuildTask.MsBuildTaskType.CONSOLE_APPLICATION, solutionProject.Path.FullPath, "bin\\" + solutionProject.Name + ".zip");
            }
            // TODO: migrations project
            return null;
            
        }

        
    }
}
