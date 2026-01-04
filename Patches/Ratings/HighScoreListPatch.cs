using HarmonyLib;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(HighScoreList))]
public class HighScoreListPatch
{
    [HarmonyPatch("ReplaceHighScore")]
    [HarmonyPrefix]
    static bool ReplaceHighScorePrefix(ref float accuracy, bool cleared)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        if(!cleared)
        {
            // Fix a base game bug where failing at high accuracy saves said high accuracy,
            // then applies it later when the chart is passed
            // This isn't *too* much of an issue in the base game because you have to miss to fail, but is
            // super pertinent with death link because it allows you to fail without even missing or mistiming at all
            accuracy = 0f;
        }
        return true;
    }
}