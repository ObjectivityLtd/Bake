using Cake.CD.Command;
using Cake.CD.Logging;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.CD.Templating.Steps;
using Cake.CD.Templating.Steps.Build;
using Cake.Core.IO;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cake.CD.Templating.Plan
{
    public class BuildTemplatePlanFactory
    {

        private SolutionInfoProvider solutionInfoProvider;

        private List<ISolutionScriptTaskFactory> solutionScriptTaskFactories;

        private List<IProjectScriptTaskFactory> projectScriptTaskFactories;

        private TemplateFileProvider templateFileProvider;

        private ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildTemplatePlanFactory(
            SolutionInfoProvider solutionInfoProvider,
            IEnumerable<ISolutionScriptTaskFactory> solutionScriptTaskFactories,
            IEnumerable<IProjectScriptTaskFactory> projectScriptTaskFactories,
            TemplateFileProvider templateFileProvider, 
            ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.solutionInfoProvider = solutionInfoProvider;
            this.solutionScriptTaskFactories = solutionScriptTaskFactories.OrderBy(stf => stf.Order).ToList();
            this.projectScriptTaskFactories = projectScriptTaskFactories.OrderBy(stf => stf.Order).ToList();
            this.templateFileProvider = templateFileProvider;
            this.scriptTaskEvaluator = scriptTaskEvaluator;
            
        }

        public TemplatePlan CreateTemplatePlan(InitOptions initOptions)
        {
            var buildCakeTask = new BuildCake(scriptTaskEvaluator, initOptions);
            var templatePlan = this.CreateBaseTemplatePlan(buildCakeTask, initOptions.Overwrite);
            if (initOptions.SolutionFilePath == null)
            {
                // TODO: project explorer
                Log.Warning("No solution file provided - creating default templates.");
                return templatePlan;
            }
            var relativeSlnDir = new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(initOptions.SolutionFilePath);
            LogHelper.LogHeader("Exploring projects - parsing solution {SlnFile}.", relativeSlnDir.FullPath);
            LogHelper.IncreaseIndent();
            var solutionInfo = solutionInfoProvider.ParseSolution(initOptions);
            var tasks = new List<IScriptTask>();
            tasks.AddRange(CreateSolutionLevelTasks(solutionInfo));
            tasks.AddRange(CreateProjectLevelTasks(solutionInfo));
            buildCakeTask.AddScriptTasks(tasks.OrderBy(task => task.Type.TaskOrder));
            LogHelper.DecreaseIndent();
            return templatePlan;
        }

        private TemplatePlan CreateBaseTemplatePlan(BuildCake buildCakeTask, bool shouldOverwrite)
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan.AddStep(new CopyFileStep(templateFileProvider, "build\\build.ps1", "build\\build.ps1", shouldOverwrite));
            templatePlan.AddStep(buildCakeTask);
            return templatePlan;
        }

        private IEnumerable<IScriptTask> CreateSolutionLevelTasks(SolutionInfo solutionInfo)
        {
            var result = new List<IScriptTask>();
            var scriptTaskFactories = solutionScriptTaskFactories.Where(stf => stf.IsApplicable(solutionInfo)).OrderBy(stf => stf.Order);
            foreach (var scriptTaskFactory in scriptTaskFactories)
            {
                var projectType = scriptTaskFactory.GetType().Name.Replace("Factory", "");
                Log.Information("Preparing solution-level task {Task}.", projectType);
                var scriptTasks = scriptTaskFactory.Create(solutionInfo);
                var uniqueTasks = GetNewUniqueScriptTasks(result, scriptTasks);
                result.AddRange(uniqueTasks);
            }
            return result;
        }


        private IEnumerable<IScriptTask> CreateProjectLevelTasks(SolutionInfo solutionInfo)
        {
            var result = new List<IScriptTask>();
            foreach (var projectInfo in solutionInfo.Projects)
            {
                var scriptTasks = CreateProjectTasks(projectInfo);
                var uniqueTasks = GetNewUniqueScriptTasks(result, scriptTasks);
                result.AddRange(uniqueTasks);

            }
            return result;
        }

        private IEnumerable<IScriptTask> CreateProjectTasks(ProjectInfo projectInfo)
        {
            var result = new List<IScriptTask>();
            var scriptTaskFactories = projectScriptTaskFactories.Where(stf => stf.IsApplicable(projectInfo)).OrderBy(stf => stf.Order);
            foreach (var scriptTaskFactory in scriptTaskFactories)
            {
                var projectType = scriptTaskFactory.GetType().Name.Replace("Factory", "");
                Log.Information("Found {ProjectType} project at {ProjectPath}.", projectType, new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(projectInfo.Project.Path));
                var scriptTasks = scriptTaskFactory.Create(projectInfo);
                result.AddRange(scriptTasks);
            }
            return result;
        }

        private IEnumerable<IScriptTask> GetNewUniqueScriptTasks(IEnumerable<IScriptTask> existingScriptTasks, IEnumerable<IScriptTask> newScriptTasks)
        {
            var existingNames = existingScriptTasks.Select(scriptTask => scriptTask.Name).ToList();
            return newScriptTasks.Where(scriptTask => !existingNames.Contains(scriptTask.Name));
        }
    }
}
