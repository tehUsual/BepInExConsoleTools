using System;
using System.Diagnostics;
using BepInEx.Logging;

namespace ConsoleTools;

public static class ManualLogSourceExtensions
{
    /// <summary>
    /// Logs a message to the provided <see cref="ManualLogSource"/> in a specified console color.
    /// </summary>
    /// <param name="log">The <see cref="ManualLogSource"/> instance to use for logging.</param>
    /// <param name="message">The message to be logged.</param>
    /// <param name="color">The color to display the logged message in. Defaults to <see cref="ConsoleColor.Gray"/>.</param>
    /// <param name="callerInfo">
    /// Boolean indicating whether to include the caller information (class and method name) in the log message.
    /// Defaults to <c>true</c>.
    /// </param>
    /// <param name="oneColor">
    /// Boolean flag to specify usage of a single consistent color throughout the message.
    /// Defaults to <c>false</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="log"/> parameter is <c>null</c>.</exception>
    public static void LogColor(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.Gray,
        bool callerInfo = true, bool oneColor = false)
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
        
        if (oneColor)
        {
            string code = $"#CC{(int)color:D2}";
            log.LogInfo($"{code}{callerInfoStr}{message}");
        }
        else
        {
            string sourceColorCode = $"#CS{((int)ConsoleConfig.SourceColor):D2}";
            string callerColorCode = $"#CS{((int)ConsoleConfig.CallerColor):D2}";
            string msgColorCode = $"#CS{(int)color:D2}";
            
            log.LogInfo($"{sourceColorCode}{callerColorCode}{callerInfoStr}{msgColorCode}{message}");
        }
    }
}