using System;
using System.Collections.Generic;

namespace ConsoleTools;

public static class ConsoleConfig
{
    // Unity Logs
    public static bool ShowUnityLogs { get; set; } = true;
    
    // Registered plugins
    //internal static readonly HashSet<string> RegisteredPlugins = new(StringComparer.OrdinalIgnoreCase);
    internal static readonly Dictionary<string, bool> RegisteredPlugins = new(StringComparer.OrdinalIgnoreCase);
    
    // Color config
    internal static ConsoleColor SourceColor { get; private set; } = ConsoleColor.Gray;
    internal static ConsoleColor CallerColor { get; private set; } = ConsoleColor.Gray;
    
    
    /// <summary>
    /// Registers a plugin by adding it to the registered plugins list.
    /// </summary>
    /// <param name="pluginName">The name of the plugin to be registered.</param>
    public static void Register(string pluginName)
    {
        RegisteredPlugins[pluginName] = true;
    }

    /// <summary>
    /// Unregisters a plugin by removing it from the registered plugins list.
    /// </summary>
    /// <param name="pluginName">The name of the plugin to be unregistered.</param>
    public static void Unregister(string pluginName)
    {
        RegisteredPlugins.Remove(pluginName);
    }

    /// <summary>
    /// Configures the logging state for a specific plugin by enabling or disabling it.
    /// </summary>
    /// <param name="pluginName">The name of the plugin whose logging state is to be modified.</param>
    /// <param name="enable">A boolean value indicating whether to enable (true) or disable (false) logging for the specified plugin.</param>
    public static void SetLogging(string pluginName, bool enable)
    {
        if (!string.IsNullOrEmpty(pluginName))
        {
            RegisteredPlugins[pluginName] = enable;
        }
    }

    public static void SetDefaultSourceColor(ConsoleColor color) => SourceColor = color;
    public static void SetDefaultCallerColor(ConsoleColor color) => CallerColor = color;
}