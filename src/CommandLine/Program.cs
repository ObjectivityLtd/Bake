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
                Config.ConfigureLogger();
                var container = Config.ConfigureAutofacContainer();
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
    }
}

