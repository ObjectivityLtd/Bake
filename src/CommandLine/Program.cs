using Autofac;
using Cake.CD.Cake;
using Cake.CD.Command;
using Cake.CD.Templating;
using Cake.Core.Composition;
using Cake.Core.Modules;
using Serilog;
using System;
using System.Reflection;

namespace Cake.CD.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            //try
            //{
                ConfigureLogger();
                var container = ConfigureCakeCDContainer();
                using (var scope = container.BeginLifetimeScope())
                {
                    return scope.Resolve<CommandLineParser>().Parse(args);
                }
                    
            /*} catch (Exception e)
            {
                Log.Error(e, "");
                return -1;
            }*/
        }
        
        private static IContainer ConfigureCakeCDContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterAssemblyTypes(typeof(Program).GetTypeInfo().Assembly)
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .AsSelf();

            AddCakeCoreModules(builder);
            return builder.Build();
        }

        private static void AddCakeCoreModules(ContainerBuilder containerBuilder)
        {
            var cakeRegistrar = new ContainerRegistrar(containerBuilder);
            cakeRegistrar.RegisterModule(new CoreModule());
            cakeRegistrar.RegisterModule(new CakeCDModule());
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.LiterateConsole()
               .CreateLogger();
        }
    }
}

