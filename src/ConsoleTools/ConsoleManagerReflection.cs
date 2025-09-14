using System;
using System.IO;
using System.Reflection;

namespace ConsoleTools;

internal static class ConsoleManagerReflection
{
    private static readonly MethodInfo? SetConsoleColorMethod;

    private static TextWriter? ConsoleStream { get; }

    static ConsoleManagerReflection()
    {
        var bepinexAssembly = typeof(BepInEx.BaseUnityPlugin).Assembly;
        var consoleManagerType = bepinexAssembly.GetType("BepInEx.ConsoleManager");

        if (consoleManagerType == null)
            return;
        
        SetConsoleColorMethod = consoleManagerType.GetMethod(
            "SetConsoleColor",
            BindingFlags.Static | BindingFlags.Public
        );

        var consoleStreamProperty = consoleManagerType.GetProperty(
            "ConsoleStream",
            BindingFlags.Static | BindingFlags.Public
        );

        ConsoleStream = consoleStreamProperty?.GetValue(null) as TextWriter;
    }

    private static void SetColor(ConsoleColor color)
    {
        SetConsoleColorMethod?.Invoke(null, [color]);
    }

    internal static void WriteLine(string message, ConsoleColor? color = null)
    {
        if (color.HasValue)
            SetColor(color.Value);

        ConsoleStream?.WriteLine(message);

        if (color.HasValue)
            SetColor(ConsoleColor.Gray);
    }
}