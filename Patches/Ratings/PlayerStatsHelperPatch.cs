using Arcade.Utils;
using HarmonyLib;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(PlayerStatsHelper))]
public class PlayerStatsHelperPatch
{
    [HarmonyPatch("ResetSavedValues")]
    [HarmonyPostfix]
    static void ResetSavedValuesPostfix()
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        // We want to recalculate rating just like the base calculator does
        HighScoreHandler.ResetSavedRating();
    }


    [HarmonyPatch("GetPlayerRank")]
    [HarmonyPrefix]
    static bool GetPlayerRankPrefix(ref float __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        __result = HighScoreHandler.GetOverallPlayerRating();
        return false;
    }
}