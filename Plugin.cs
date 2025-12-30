using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UNBEATAP.Patches;
using UNBEATAP.AP;
using BepInEx.Configuration;
using System;
using UNBEATAP.Helpers;

namespace UNBEATAP;

[BepInPlugin(PluginReleaseInfo.PLUGIN_GUID, PluginReleaseInfo.PLUGIN_NAME, PluginReleaseInfo.PLUGIN_VERSION)]
[BepInProcess("UNBEATABLE.exe")]
public class Plugin : BaseUnityPlugin
{
    public const string GameName = "UNBEATABLE Arcade";
    public const string AssetsFolder = "BepInEx/plugins/unbeatAP/Assets";

    internal static new ManualLogSource Logger;

    public static Client Client;

    private ConfigEntry<string> configIp;
    private ConfigEntry<int> configPort;
    private ConfigEntry<string> configSlot;
    private ConfigEntry<string> configPassword;

    private ConfigEntry<bool> configDeathLink;


    private void LoadConfig()
    {
        configIp = Config.Bind(
            "Connection",
            "IP",
            "archipelago.gg",
            "The archipelago server IP to connect to. If you're running the game on the official website, this is probably 'archipelago.gg'."
        );
        configPort = Config.Bind(
            "Connection",
            "Port",
            58008,
            "The port to connect to. This is all the numbers after the ':' in the IP."
        );
        configSlot = Config.Bind(
            "Connection",
            "Slot",
            "Player1",
            "The player slot name to connect with. Enter what you set 'name' to in your YAML."
        );
        configPassword = Config.Bind(
            "Connection",
            "Password",
            "",
            "The password to use when connecting. If the server has no password, leave this empty."
        );

        configDeathLink = Config.Bind(
            "Gameplay",
            "DeathLink",
            false,
            "If Death Link is enabled, you'll fail out of a song if someone else dies, and everyone else will die when you fail a song."
        );
    }


    private async void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        LoadConfig();

        await DifficultyList.Init();

        Client = new Client(configIp.Value, configPort.Value, configSlot.Value, configPassword.Value, configDeathLink.Value);
        await Client.ConnectAndGetData();

        try
        {
            // ArcadeSongView seems pretty useless if ArcadeDifficultyView is enabled, maybe it can be unpatched on archipelago connection?
            // Harmony.CreateAndPatchAll(typeof(ArcadeSongView));

            Harmony.CreateAndPatchAll(typeof(ArcadeCharacterView));
            Harmony.CreateAndPatchAll(typeof(ArcadeDifficultyView));
            Harmony.CreateAndPatchAll(typeof(BlockAuthentication));
            Harmony.CreateAndPatchAll(typeof(UnlockAll));
        }
        catch(Exception e)
        {
            Logger.LogFatal($"Patching failed with error: {e.Message}, {e.StackTrace}");
            return;
        }

        Logger.LogInfo($"Plugin {PluginReleaseInfo.PLUGIN_GUID} is loaded!");
    }
}