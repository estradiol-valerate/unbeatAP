using HarmonyLib;
using Rhythm;
using UNBEATAP.AP;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(RhythmController))]
public class RhythmControllerPatch
{
    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    static void UpdatePostfix(RhythmController __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        if(!Plugin.Client.deathLink)
        {
            return;
        }

        if(!__instance.song.gameOver)
        {
            DeathHelper.HandledDeath = false;
            return;
        }

        if(__instance.waitBeforePausingSeconds > 0f)
        {
            DeathHelper.HandledDeath = false;
            return;
        }

        if(JeffBezosController.rhythmProgression is SetlistProgression)
        {
            DeathHelper.HandledDeath = false;
            return;
        }

        // The player death gets triggered here
        DeathHelper.OnPlayerDeath(DeathLinkReason.Fail);
        DeathHelper.HandledDeath = true;
    }
}