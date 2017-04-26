using Autofac;
using Cake.CD.Command;
using Cake.CD.Templating;
using System;
using System.Reflection;

namespace Cake.CD.CommandLine
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var container = ConfigureDependencyInjection();
                using (var scope = container.BeginLifetimeScope())
                {
                    return scope.Resolve<CommandLineParser>().Parse(args);
                }
                    
            } catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Stack trace: " + e.StackTrace);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + e.InnerException);
                }
                Console.ResetColor();
                return -1;
            }
        }
        
        static IContainer ConfigureDependencyInjection()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterAssemblyTypes(typeof(Program).GetTypeInfo().Assembly)
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .AsSelf();
            return builder.Build();
        }
    }
}

