using Autofac;
using Bake.API.ScriptTaskFactory;
using Bake.CakeConfig;
using Cake.Core.Modules;
using Serilog;
using System.Linq;
using System.Reflection;

namespace Bake
{
    internal static class Config
    {
        public static IContainer ConfigureAutofacContainer()
        {
            var builder = new ContainerBuilder();
            var assemblies = GetBakeAssemblies();
            AddBakeCommon(builder, assemblies);
            AddBakeScriptFactories(builder, assemblies);
            AddCakeCoreModules(builder);
            var result = builder.Build();
            return result;
        }

        private static void AddBakeCommon(ContainerBuilder containerBuilder, Assembly[] bakeAssemblies)
        {
            containerBuilder
                .RegisterAssemblyTypes(bakeAssemblies)
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .AsSelf();
        }

        private static void AddBakeScriptFactories(ContainerBuilder containerBuilder, Assembly[] bakeAssemblies)
        {
            containerBuilder
                .RegisterAssemblyTypes(bakeAssemblies)
                .Where(t => typeof(IProjectScriptTaskFactory).IsAssignableFrom(t) || typeof(ISolutionScriptTaskFactory).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();
        }

        private static void AddCakeCoreModules(ContainerBuilder containerBuilder)
        {
            var cakeRegistrar = new ContainerRegistrar(containerBuilder);
            cakeRegistrar.RegisterModule(new CoreModule());
            cakeRegistrar.RegisterModule(new CakeExtModule());
        }

        private static Assembly[] GetBakeAssemblies()
        {
            var currentAssembly = typeof(Config).GetTypeInfo().Assembly;
            var referencedAssemblies = currentAssembly
                .GetReferencedAssemblies()
                .Where(assemblyName => assemblyName.Name.StartsWith("Bake"))
                .Select(Assembly.Load);

            return new Assembly[] {currentAssembly}.Concat(referencedAssemblies).ToArray();

        }

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Indent}{Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
