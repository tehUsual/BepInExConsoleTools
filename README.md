# ConsoleTools
A lightweight console utility for BepInEx plugins that adds colored output and caller information.  
  
  
### Output
![ConsoleColor Example](consolecolor.png)
  
  
### Usage
```cs
using ConsoleTools;
using ConsoleTools.Patches;


public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;


	private void Awake()
	{
		Log = Logger;
		
		ConsoleConfig.Register(Name);
		ConsoleConfig.ShowUnityLogs = true;     // default: true
		ConsoleConfig.SetLogging(Name, true);   // default: true

		harmony.PatchAll(typeof(ConsoleLogListenerPatches));

		Log.LogColor("Plugin has loaded successfully!", ConsoleColor.Green);
	}

	private void Start()
	{
		Log.LogColor("Cyan is nice", ConsoleColor.Cyan);
		Log.LogColor("Uh oh (without caller info)", ConsoleColor.Yellow, false);
	}

    private void Update()
    {
        // Toggle Unity logs
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ConsoleConfig.ShowUnityLogs = !ConsoleConfig.ShowUnityLogs;
            Log.LogColor($"Unity console logs {(ConsoleConfig.ShowUnityLogs ? "enabled" : "disabled")}",
                ConsoleColor.DarkMagenta);
        }
    }
}
```
