using System;
using System.Diagnostics;
using BepInEx.Logging;

namespace ConsoleTools;

public static class ManualLogSourceExtensions
{
    /// <summary>
    /// Logs a message to a <see cref="ManualLogSource"/> with a specified color and optional caller information.
    /// </summary>
    /// <param name="log">The <see cref="ManualLogSource"/> instance to use for logging.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="color">The color to display the message in the console. Defaults to <see cref="ConsoleColor.Gray"/>.</param>
    /// <param name="callerInfo">Indicates whether to include caller information in the log. Defaults to <c>true</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="log"/> parameter is <c>null</c>.</exception>
    public static void LogColor(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.Gray,
        bool callerInfo = true)
    {
        if (log == null) throw new ArgumentNullException(nameof(log));

        string callerInfoStr = "";
        if (callerInfo)
        {
            var stackTrace = new StackTrace(skipFrames: 1, fNeedFileInfo: false);
            var frame = stackTrace.GetFrame(0);
            var method = frame.GetMethod();
            string className = method.DeclaringType?.Name ?? "<unknown>";
            string methodName = method.Name;

            callerInfoStr = $"[{className}.{methodName}] ";
        }

        int colorValue = (int)color;
        string code = $"#CC{colorValue:D2}";
        log.LogInfo($"{code}{callerInfoStr}{message}");
    }
}