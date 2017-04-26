using Cake.Common.Solution;
using Cake.Common.Solution.Project;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using System;

namespace Cake.CD.Cake
{
    public class CakeCDModule : ICakeModule
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
        }
    }
}
