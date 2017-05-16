using Bake.API.Task;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Serilog;
using System;
using System.Reflection;

namespace Bake.Scripting
{
    public class RoslynScriptEvaluator : IScriptEvaluator
    {

        public string Evaluate(ITask task, IScriptState scriptState, string scriptBody)
        {
            var options = ScriptOptions.Default;
            options = AddSystemImports(options);
            options = AddCakeApiImports(options);
            options = AddCakeModuleImports(options, task, scriptState);
                
            object result = null;
            var currentTaskName = scriptState.CurrentTask == null ? "<none>" : scriptState.CurrentTask.Name;
            var currentTaskType = scriptState.CurrentTask == null ? "" : scriptState.CurrentTask.GetType().Name;
            try
            {
                CSharpScript.EvaluateAsync(scriptBody, options, scriptState)
                     .ContinueWith(s => result = s.Result)
                     .Wait();
            } catch (CompilationErrorException e)
            {
                Log.Error("Failed to compile script {Type} '{Name}': {Diag}", 
                    currentTaskType, currentTaskName, string.Join(Environment.NewLine, e.Diagnostics));
                throw;
            } catch (Exception e)
            {
                Log.Error("Failed to execute script {Type} '{Name}'", currentTaskType, currentTaskName, e);
                throw;
            }
            return (result as string)?.TrimEnd();
        }

        private ScriptOptions AddSystemImports(ScriptOptions scriptOptions)
        {
            return scriptOptions
                .AddImports("System")
                .AddImports("System.Linq")
                .AddImports("System.Environment")
                .AddImports("System.IO")
                .AddImports("System.Collections.Generic");
        }

        private ScriptOptions AddCakeApiImports(ScriptOptions scriptOptions)
        {
            return scriptOptions
                .AddReferences(typeof(ITask).GetTypeInfo().Assembly)
                .AddImports("Bake.API.Task");
        }

        private ScriptOptions AddCakeModuleImports(ScriptOptions scriptOptions, ITask task, IScriptState scriptState)
        {
            return scriptOptions
                .AddReferences(task.GetType().GetTypeInfo().Assembly)
                .AddImports(scriptState.GetType().Namespace)
                .AddImports(task.GetType().Namespace);
        }
    }
}
