using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Rhythm;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(BeatmapIndex))]
public class BeatmapIndexPatch
{
    [HarmonyPatch(nameof(BeatmapIndex.VisibleDifficulties), MethodType.Getter)]
    [HarmonyPrefix]
    static bool GetVisibleDifficultiesPrefix(ref string[] __result, BeatmapIndex __instance)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        string[] baseDiffs = new Traverse(__instance).Field("_selectableDifficulties").GetValue<HashSet<string>>().ToArray();
        foreach(string diff in baseDiffs)
        {
            Plugin.Logger.LogInfo(diff);
        }

        __result = DifficultyList.GetVisibleDifficulties().ToArray();
        return false;
    }
}