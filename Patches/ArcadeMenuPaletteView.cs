using System.Collections.Generic;
using Arcade.Progression;
using HarmonyLib;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(MenuPaletteUnlocksManager))]
public class ArcadeMenuPaletteView
{
    [HarmonyPatch("GetUnlockState")]
    [HarmonyPrefix]
    static bool GetUnlockStatePrefix(MenuPaletteUnlocksManager __instance, string unlock)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        // We don't want to unlock all the menu palettes, so ignore MainProgressionContainer.UnlockAll
        // It would be cool for the player to have access to all of them, but it's not necessary for the rando
        // and it leads to a bit of jank when disconnecting due to selecting a palette you no longer have unlocked
        Dictionary<string, bool> _lockState = new Traverse(__instance).Field("_lockState").GetValue<Dictionary<string, bool>>();
        return _lockState[unlock];
    }
}