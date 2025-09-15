# ConsoleTools
A lightweight console utility for BepInEx plugins that adds colored output and caller information.  
  
  
### Output
![ConsoleColor Example](consolecolor.png)
  
  
  
### Install
Clone the repo, open your projects *.csproj and add this:
```xml
    <ItemGroup>
        <Compile Include="..\..\..\ConsoleTools\src\ConsoleTools\**\*.cs">
            <Link>ConsoleTools\%(RecursiveDir:ConsoleTools\)%(Filename)%(Extension)</Link>
        </Compile>
    </ItemGroup>
```
  
  

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

        Log.LogColor("This is a green full line", ConsoleColor.Green, oneColor: true);
        Log.LogColor("This is a cyan full line", ConsoleColor.Cyan, oneColor: true);
        
        ConsoleConfig.SetDefaultCallerColor(ConsoleColor.DarkYellow);
        Log.LogColor("This is a cyan line with a yellow caller", ConsoleColor.Cyan);
        Log.LogColor("This is a cyan line without a caller", ConsoleColor.Cyan, callerInfo: false);
        
        ConsoleConfig.SetDefaultSourceColor(ConsoleColor.DarkRed);
        Log.LogColor("This is a green line with a yellow caller and a red source", ConsoleColor.DarkGreen);


		// Set default source and caller config for all ConsoleConfig.LogColor()
        ConsoleConfig.SetDefaultSourceColor(ConsoleColor.DarkCyan);
        ConsoleConfig.SetDefaultCallerColor(ConsoleColor.DarkYellow);
		
		// Logging level
		Log.LogColor("msg");	//	LogLevel.Info		-	defaults to gray
		Log.LogColorW("msg");	//	LogLevel.Warning	-	defaults to dark yellow
		Log.LogColorE("msg");	//	LogLevel.Error		-	defaults to dark red
		Log.LogColorF("msg");	//	LogLevel.Fatal		-	defaults to dark red
		Log.LogColorD("msg");	//	LogLevel.Debug		-	defaults to gray
		Log.LogColorS("msg");	//	LogLevel.Info		-	defaults to dark green (success)
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
