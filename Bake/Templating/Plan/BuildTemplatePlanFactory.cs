using Bake.API.EntryScript;
using Bake.API.Logging;
using Bake.API.ScriptTaskFactory;
using Bake.API.Task;
using Bake.Cake.Build.EntryScript;
using Bake.Templating.Step;
using Cake.Core.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bake.Templating.Plan
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
                Log.Warn("No solution file provided - creating default templates.");
                return templatePlan;
            }
            var relativeSlnDir = new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(initOptions.SolutionFilePath);
            Log.Header("Exploring projects - parsing solution {SlnFile}.", relativeSlnDir.FullPath);
            Log.IncreaseIndent();
            var solutionInfo = solutionInfoProvider.ParseSolution(initOptions);
            var tasks = new List<ITask>();
            tasks.AddRange(CreateSolutionLevelTasks(solutionInfo));
            tasks.AddRange(CreateProjectLevelTasks(solutionInfo));
            buildCakeTask.AddScriptTasks(tasks.OrderBy(task => task.Type.TaskOrder));
            Log.DecreaseIndent();
            return templatePlan;
        }

        private TemplatePlan CreateBaseTemplatePlan(BuildCake buildCakeTask, bool shouldOverwrite)
        {
            TemplatePlan templatePlan = new TemplatePlan();
            templatePlan.AddStep(new CreateFileFromTemplate(templateFileProvider, "build\\build.ps1", "build\\build.ps1", shouldOverwrite));
            templatePlan.AddStep(buildCakeTask);
            return templatePlan;
        }

        private IEnumerable<ITask> CreateSolutionLevelTasks(SolutionInfo solutionInfo)
        {
            var result = new List<ITask>();
            var scriptTaskFactories = solutionScriptTaskFactories.Where(stf => stf.IsApplicable(solutionInfo)).OrderBy(stf => stf.Order);
            foreach (var scriptTaskFactory in scriptTaskFactories)
            {
                var projectType = scriptTaskFactory.GetType().Name.Replace("Factory", "");
                Log.Info("Preparing solution-level task {Task}.", projectType);
                var scriptTasks = scriptTaskFactory.Create(solutionInfo);
                var uniqueTasks = GetNewUniqueScriptTasks(result, scriptTasks);
                result.AddRange(uniqueTasks);
            }
            return result;
        }


        private IEnumerable<ITask> CreateProjectLevelTasks(SolutionInfo solutionInfo)
        {
            var result = new List<ITask>();
            foreach (var projectInfo in solutionInfo.Projects)
            {
                var scriptTasks = CreateProjectTasks(projectInfo);
                var uniqueTasks = GetNewUniqueScriptTasks(result, scriptTasks);
                result.AddRange(uniqueTasks);

            }
            return result;
        }

        private IEnumerable<ITask> CreateProjectTasks(ProjectInfo projectInfo)
        {
            var result = new List<ITask>();
            var scriptTaskFactories = projectScriptTaskFactories.Where(stf => stf.IsApplicable(projectInfo)).OrderBy(stf => stf.Order);
            foreach (var scriptTaskFactory in scriptTaskFactories)
            {
                var projectType = scriptTaskFactory.GetType().Name.Replace("Factory", "");
                Log.Info("Found {ProjectType} project at {ProjectPath}.", projectType, new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(projectInfo.Project.Path).FullPath);
                var scriptTasks = scriptTaskFactory.Create(projectInfo);
                result.AddRange(scriptTasks);
            }
            return result;
        }

        private IEnumerable<ITask> GetNewUniqueScriptTasks(IEnumerable<ITask> existingScriptTasks, IEnumerable<ITask> newScriptTasks)
        {
            var existingNames = existingScriptTasks.Select(scriptTask => scriptTask.Name).ToList();
            return newScriptTasks.Where(scriptTask => !existingNames.Contains(scriptTask.Name));
        }
    }
}
