using Serilog.Context;
using System;

namespace Bake.API.Logging
{
    public static class Log
    {
        private static string indent = "";

        private static IDisposable currentIndentProperty = null;

        public static void IncreaseIndent()
        {
            indent += "  ";
            UpdateIndent();
        }

        public static void DecreaseIndent()
        {
            if (indent.Length == 0)
            {
                return;
            }
            indent = indent.Remove(indent.Length - 2);
            UpdateIndent();
        }

        public static void Info(string messageTemplate, params object[] propertyValues)
        {
            Serilog.Log.Information(messageTemplate, propertyValues);
        }

        public static void Warn(string messageTemplate, params string[] propertyValues)
        {
            Serilog.Log.Warning(messageTemplate, propertyValues);
        }

        public static void Error(Exception exception, string messageTemplate, params string[] propertyValues)
        {
            Serilog.Log.Error(exception, messageTemplate, propertyValues);
        }

        public static void Header(string messageTemplate, params string[] propertyValues)
        {
            Serilog.Log.Information("");
            Serilog.Log.Information("{prefix} " + messageTemplate, "==========", propertyValues);
        }

        private static void UpdateIndent()
        {
            currentIndentProperty?.Dispose();
            currentIndentProperty = LogContext.PushProperty("Indent", indent);
        }
    }
}
