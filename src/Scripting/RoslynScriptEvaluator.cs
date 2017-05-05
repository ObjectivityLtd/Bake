using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Cake.CD.Scripting
{
    public class RoslynScriptEvaluator : IScriptEvaluator
    {

        public string Evaluate(string scriptBody, IScriptState scriptState)
        {
           return CSharpScript.EvaluateAsync(scriptBody, globals: scriptState).Result as string;
        }
    }
}
