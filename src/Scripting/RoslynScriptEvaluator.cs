using Cake.CD.Templating;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Serilog;
using System;
using System.Reflection;

namespace Cake.CD.Scripting
{
    public class RoslynScriptEvaluator : IScriptEvaluator
    {

        public string Evaluate(string scriptBody, IScriptState scriptState)
        {
            var options = ScriptOptions.Default
                .AddReferences(typeof(IScriptTask).GetTypeInfo().Assembly)
                .AddImports("System")
                .AddImports("System.Environment")
                .AddImports("System.IO")
                .AddImports("System.Collections.Generic")
                .AddImports("Cake.CD.Templating")
                .AddImports(scriptState.GetType().Namespace);
                
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
    }
}
