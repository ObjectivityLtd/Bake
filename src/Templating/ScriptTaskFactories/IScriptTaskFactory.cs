using System.Collections.Generic;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;

namespace Cake.CD.Templating.ScriptTaskFactories
{
    public interface IScriptTaskFactory
    {
        IEnumerable<IScriptTask> Create(SolutionProject solutionProject, ProjectParserResult parserResult);
        bool IsApplicable(SolutionProject solutionProject, ProjectParserResult parserResult);
    }
}