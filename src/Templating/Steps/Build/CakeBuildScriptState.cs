using System;
using System.Collections.Generic;

namespace Cake.CD.Templating.Steps.Build
{
    public class CakeBuildScriptState : BuildScriptState
    {
        private readonly ISet<string> cakeAddins = new HashSet<string>();

        public CakeBuildScriptState(ScriptTaskEvaluator scriptTaskEvaluator, string buildScriptPath, string outputPath)
            : base(scriptTaskEvaluator, buildScriptPath, outputPath)
        {
            
        }

        public string AddCakeAddin(string addinName)
        {
            if (cakeAddins.Contains(addinName))
            {
                return "";
            }
            cakeAddins.Add(addinName);
            return $"#addin \"{addinName}\"{Environment.NewLine}";
        }
    }
}
