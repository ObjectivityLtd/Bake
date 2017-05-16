using Cake.Core.Diagnostics;
using Serilog;
using Serilog.Events;

namespace Bake.CakeConfig
{
    public class CakeLog : ICakeLog
    {
        public Verbosity Verbosity { get; set; }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }
            LogEventLevel seriLevel = ConvertToSerilogLevel(level);
            Log.Write(seriLevel, format, args);
        }

        private LogEventLevel ConvertToSerilogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug: return LogEventLevel.Debug;
                case LogLevel.Verbose: return LogEventLevel.Verbose;
                case LogLevel.Information: return LogEventLevel.Information;
                case LogLevel.Warning: return LogEventLevel.Warning;
                case LogLevel.Error: return LogEventLevel.Error;
            }
            return LogEventLevel.Information;
        }
    }
}
