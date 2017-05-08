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
                .AddImports("System.IO")
                .AddImports("Cake.CD.Templating")
                .AddImports(scriptState.GetType().Namespace);
                
            object result = null;
            try
            {
                CSharpScript.EvaluateAsync(scriptBody, options: options, globals: scriptState)
                     .ContinueWith(s => result = s.Result)
                     .Wait();
            } catch (CompilationErrorException e)
            {
                var currentTaskName = scriptState.CurrentTask == null ? "<none>" : scriptState.CurrentTask.Name;
                Log.Error("Failed to compile script {Name}: {Diag}", currentTaskName, string.Join(Environment.NewLine, e.Diagnostics));
                throw e;
            }
            return result as string;
        }   
    }
}
