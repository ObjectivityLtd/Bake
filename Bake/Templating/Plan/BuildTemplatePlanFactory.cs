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

        private ProjectInfoProvider projectInfoProvider;

        private List<ISolutionScriptTaskFactory> solutionScriptTaskFactories;

        private List<IProjectScriptTaskFactory> projectScriptTaskFactories;

        private TemplateFileProvider templateFileProvider;

        private ScriptTaskEvaluator scriptTaskEvaluator;

        public BuildTemplatePlanFactory(
            ProjectInfoProvider projectInfoProvider,
            IEnumerable<ISolutionScriptTaskFactory> solutionScriptTaskFactories,
            IEnumerable<IProjectScriptTaskFactory> projectScriptTaskFactories,
            TemplateFileProvider templateFileProvider, 
            ScriptTaskEvaluator scriptTaskEvaluator)
        {
            this.projectInfoProvider = projectInfoProvider;
            this.solutionScriptTaskFactories = solutionScriptTaskFactories.OrderBy(stf => stf.Order).ToList();
            this.projectScriptTaskFactories = projectScriptTaskFactories.OrderBy(stf => stf.Order).ToList();
            this.templateFileProvider = templateFileProvider;
            this.scriptTaskEvaluator = scriptTaskEvaluator;
        }

        public TemplatePlan CreateTemplatePlan(InitOptions initOptions)
        {
            var buildCakeTask = new BuildCake(scriptTaskEvaluator, initOptions);
            var templatePlan = this.CreateBaseTemplatePlan(buildCakeTask, initOptions.Overwrite);
            var solutionInfo = PrepareSolutionInfo(initOptions);
            var projects = FindProjects(solutionInfo);

            var projectTasks = CreateProjectLevelTasks(solutionInfo, projects);
            var solutionTasks = CreateSolutionLevelTasks(solutionInfo);

            var tasks = new List<ITask>();
            tasks.AddRange(solutionTasks);
            tasks.AddRange(projectTasks);            
            buildCakeTask.AddScriptTasks(tasks.OrderBy(task => task.Type.TaskOrder));
            Log.DecreaseIndent();
            return templatePlan;
        }

        private SolutionInfo PrepareSolutionInfo(InitOptions initOptions)
        {
            if (initOptions.SolutionFilePath == null)
            {
                var currentDir = new DirectoryPath(Directory.GetCurrentDirectory());
                Log.Header("Exploring projects - searching directory {CurrentDir}.", currentDir.FullPath);
                Log.IncreaseIndent();
                return new SolutionInfo(solutionPath: currentDir, buildSolution: false);
            }
            var relativeSlnDir = new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(initOptions.SolutionFilePath);
            Log.Header("Exploring projects - parsing solution file {SlnFile}.", relativeSlnDir.FullPath);
            return new SolutionInfo(initOptions.SolutionFilePath, initOptions.BuildSolution);
        }

        private IEnumerable<ProjectInfo> FindProjects(SolutionInfo solutionInfo)
        {
            if (solutionInfo.HasSlnFile)
            {
                return projectInfoProvider.ParseSolutionFile(solutionInfo);
            }
            return projectInfoProvider.ExploreDirectory(solutionInfo);
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


        private IEnumerable<ITask> CreateProjectLevelTasks(SolutionInfo solutionInfo, IEnumerable<ProjectInfo> projects)
        {
            var result = new List<ITask>();
            foreach (var projectInfo in projects)
            {
                solutionInfo.AddProject(projectInfo);
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
                Log.Info("Found {ProjectType} project at {ProjectPath}.", projectType, new DirectoryPath(Directory.GetCurrentDirectory()).GetRelativePath(projectInfo.Path).FullPath);
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
