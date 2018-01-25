using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TgDelayedShitpostBot
{
    public class GarbageFunctionality
    {
        public static void Log(Exception ex)
        {
            TraceMsg("Error: " + ex.Message);
        }

        public static void Log(string str)
        {
            TraceMsg(str);
        }

        private static void TraceMsg(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
