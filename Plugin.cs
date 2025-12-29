using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Arcade.Progression;
using UNBEATAP.Patches;

namespace UNBEATAP;

[BepInPlugin(PluginReleaseInfo.PLUGIN_GUID, PluginReleaseInfo.PLUGIN_NAME, PluginReleaseInfo.PLUGIN_VERSION)]
[BepInProcess("UNBEATABLE.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static float MaxScheduleOffset => RhythmConsts.LeniencyMilliseconds + 20f;
    public static float MinScheduleOffset = 20f;

        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        //Harmony.CreateAndPatchAll(typeof(ArcadeSongView));
        Harmony.CreateAndPatchAll(typeof(BlockAuthentication));
        Harmony.CreateAndPatchAll(typeof(UnlockAll));
        Logger.LogInfo($"Plugin {PluginReleaseInfo.PLUGIN_GUID} is loaded!");
        
    }

    private void Start()
    {
        Logger.LogInfo($"UnlockAll state: {MainProgressionContainer.UnlockAll}");
    }
}