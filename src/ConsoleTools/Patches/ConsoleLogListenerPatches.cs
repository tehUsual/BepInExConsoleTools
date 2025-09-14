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
        ConsoleColor defaultColor = ConsoleColor.Gray;

        if (message.StartsWith("#CC") && message.Length >= 5)
        {
            // Single-color line
            if (int.TryParse(message.Substring(3, 2), out int colorValue))
            {
                defaultColor = (ConsoleColor)colorValue;
                message = message.Substring(5);
            }
            
            ConsoleManagerReflection.Write($"[Info   :{eventArgs.Source.SourceName}] {message}", defaultColor);
        }
        else if (message.StartsWith("#CS") && message.Length >= 5)
        {
            int index = 0;
            ConsoleColor currentColor = defaultColor;

            // Check if the first #CS code exists at the start for prefix
            if (int.TryParse(message.Substring(3, 2), out int prefixColorValue))
            {
                currentColor = (ConsoleColor)prefixColorValue;
                index = 5; // skip the first #CSxx
            }

            // Print prefix with the first color
            string prefix = $"[Info   :{eventArgs.Source.SourceName}] ";
            ConsoleManagerReflection.Write(prefix, currentColor, false);

            // Process the rest of the message
            while (index < message.Length)
            {
                if (message.Length - index >= 5 && message.Substring(index, 3) == "#CS")
                {
                    if (int.TryParse(message.Substring(index + 3, 2), out int colorValue))
                    {
                        currentColor = (ConsoleColor)colorValue;
                        index += 5; // skip this color code
                        continue;
                    }
                }

                // Find the next #CS or end of string
                int nextCode = message.IndexOf("#CS", index, StringComparison.Ordinal);
                int length = (nextCode == -1 ? message.Length : nextCode) - index;

                string segmentText = message.Substring(index, length);
                ConsoleManagerReflection.Write(segmentText, currentColor, false);

                index += length;
            }

            // Finish the line
            ConsoleManagerReflection.Write("", defaultColor);
        }


        return false; // prevent default log handling
    }
}