using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx.Logging;
using HarmonyLib;

namespace ConsoleTools.Patches;

[HarmonyPatch(typeof(ConsoleLogListener))]
public static class ConsoleLogListenerPatches
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [HarmonyPatch(nameof(ConsoleLogListener.LogEvent))]
    private static bool Prefix(ConsoleLogListener __instance, object sender, LogEventArgs eventArgs)
    {
        // Allow mods to filter Unity logs
        if (eventArgs.Source.SourceName == "Unity Log" && !ConsoleConfig.ShowUnityLogs)
            return false;

        
        // Color registered plugin logs
        if (!ConsoleConfig.RegisteredPlugins.TryGetValue(eventArgs.Source.SourceName, out bool loggingEnabled) || !loggingEnabled)
            return true;
        
        string message = eventArgs.Data.ToString();
        ConsoleColor color = ConsoleColor.Gray;

        if (message.StartsWith("#CC") && message.Length >= 5)
        {
            if (int.TryParse(message.Substring(3, 2), out int colorValue))
            {
                color = (ConsoleColor)colorValue;
                message = message.Substring(5);
            }
        }

        ConsoleManagerReflection.WriteLine($"[Info   :{eventArgs.Source.SourceName}] {message}", color);
        return false; // prevent default log handling
    }
}