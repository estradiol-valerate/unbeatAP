using HarmonyLib;
using Rewired;
using UNBEATAP.AP;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(PauseMenu))]
static class PauseMenuPatch
{
    [HarmonyPatch("OptionRender")]
    [HarmonyPrefix]
    static bool OptionRenderPrefix(PauseMenu __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!Plugin.Client.deathLink)
        {
            return true;
        }

        Traverse traverse = new Traverse(__instance);
        PauseSubMenu subMenu = traverse.Field("subMenu").GetValue<PauseSubMenu>();
        if(subMenu != PauseSubMenu.Quit)
        {
            return true;
        }

        WrapCounter selection = traverse.Field("selection").GetValue<WrapCounter>();
        PauseQuitAction quitAction = (PauseQuitAction)selection.value;
        if(quitAction != PauseQuitAction.Yes)
        {
            return true;
        }

        if(ReInput.players.GetPlayer(0).GetButtonDown("Interact"))
        {
            // The player has selected the quit button, so trigger death link
            DeathHelper.OnPlayerDeath(DeathLinkReason.Quit);
        }
        return true;
    }
}