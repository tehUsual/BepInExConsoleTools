using System;
using System.Diagnostics;
using BepInEx.Logging;

namespace ConsoleTools;

public static class ManualLogSourceExtensions
{
    private static void LogColorX(this ManualLogSource log, string message, LogLevel logLevel = LogLevel.Info)
    {
        switch (logLevel)
        {
            case LogLevel.Info: log.LogInfo($"{message}"); break;       // default Gray
            case LogLevel.Warning: log.LogWarning($"{message}"); break; // default DarkYellow
            case LogLevel.Error: log.LogError($"{message}"); break;     // default Red
            case LogLevel.Fatal: log.LogFatal($"{message}"); break;     // default Red
            case LogLevel.Debug: log.LogDebug($"{message}"); break;     // default Gray
            case LogLevel.None: log.LogInfo($"{message}"); break;       // default DarkGreen
            case LogLevel.Message:
            case LogLevel.All:
            default: log.LogInfo($"{message}"); break;
        }
    }


    private static string ParseColor(string message, ConsoleColor color, bool callerInfo , bool oneColor)
    {
        string callerInfoStr = "";
        if (callerInfo)
        {
            var stackTrace = new StackTrace(skipFrames: 2, fNeedFileInfo: false);
            var frame = stackTrace.GetFrame(0);
            var method = frame.GetMethod();
            string className = method.DeclaringType?.Name ?? "<unknown>";
            string methodName = method.Name;

            callerInfoStr = $"[{className}.{methodName}] ";
        }
        
        if (oneColor)
        {
            string code = $"#CC{(int)color:D2}";
            return $"{code}{callerInfoStr}{message}";
        }
        
        string sourceColorCode = $"#CS{((int)ConsoleConfig.SourceColor):D2}";
        string callerColorCode = $"#CS{((int)ConsoleConfig.CallerColor):D2}";
        string msgColorCode = $"#CS{(int)color:D2}";

        return $"{sourceColorCode}{callerColorCode}{callerInfoStr}{msgColorCode}{message}";
    }

    public static void LogColor(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.Gray,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogInfo(message);
            return;
        }
        
        LogColorX(log, ParseColor(message, color, callerInfo, oneColor));
    }

    public static void LogColorW(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.DarkYellow,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogWarning(message);
            return;
        }
        
        LogColorX(log, ParseColor(message, color, callerInfo, oneColor), LogLevel.Warning);
    }
    
    public static void LogColorE(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.DarkRed,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogError(message);
            return;
        }

        LogColorX(log, ParseColor(message, color, callerInfo, oneColor), LogLevel.Error);
    }
    
    public static void LogColorF(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.DarkRed,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogFatal(message);
            return;
        }

        LogColorX(log, ParseColor(message, color, callerInfo, oneColor), LogLevel.Fatal);
    }
    
    public static void LogColorD(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.Gray,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogDebug(message);
            return;
        }

        LogColorX(log, ParseColor(message, color, callerInfo, oneColor), LogLevel.Debug);
    }
    
    public static void LogColorS(this ManualLogSource log, string message, ConsoleColor color = ConsoleColor.DarkGreen,
        bool callerInfo = true, bool oneColor = false)
    {
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(log.SourceName, out bool loggingEnabled) || !loggingEnabled)
        {
            log.LogInfo(message);
            return;
        }
        
        LogColorX(log, ParseColor(message, color, callerInfo, oneColor), LogLevel.None);
    }
}