using Autofac;
using Bake.CommandLine;

namespace Bake
{
    public class Program
    {
        public static int Main(string[] args)
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

