using Bake.Scripting;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using System;
using Cake.Common.Solution;
using Cake.Common.Solution.Project;

namespace Bake.CakeConfig
{
    public class CakeExtModule : ICakeModule
    {

        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            registrar.RegisterType<CakeLog>().As<ICakeLog>().Singleton();
            registrar.RegisterType<ProjectParser>().Singleton().AsSelf();
            registrar.RegisterType<SolutionParser>().Singleton().AsSelf();

            registrar.RegisterType<RoslynScriptEvaluator>().As<IScriptEvaluator>().Singleton();

        }
    }
}
