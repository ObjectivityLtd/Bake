using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Cake.CD.Cake;
using Cake.CD.Templating.ScriptTaskFactories;
using Cake.Core.Modules;
using Serilog;

namespace Cake.CD.CommandLine
{
    internal static class Config
    {
        public static IContainer ConfigureAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterAssemblyTypes(typeof(Program).GetTypeInfo().Assembly)
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .AsSelf();

            builder
                .RegisterAssemblyTypes(typeof(Program).GetTypeInfo().Assembly)
                .Where(t => typeof(IScriptTaskFactory).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();

            AddCakeCoreModules(builder);
            var result = builder.Build();
            return result;
        }

        private static void AddCakeCoreModules(ContainerBuilder containerBuilder)
        {
            var cakeRegistrar = new ContainerRegistrar(containerBuilder);
            cakeRegistrar.RegisterModule(new CoreModule());
            cakeRegistrar.RegisterModule(new CakeCDModule());
        }

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Indent}{Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
