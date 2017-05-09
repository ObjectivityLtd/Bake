using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Context;

namespace Cake.CD.Logging
{
    public static class LogHelper
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

        private static void UpdateIndent()
        {
            currentIndentProperty?.Dispose();
            currentIndentProperty = LogContext.PushProperty("Indent", indent);
        }
    }
}
