using Arcade.Progression;
using HarmonyLib;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

public class ArcadeDifficultyView
{
    [HarmonyPatch(typeof(DifficultyUnlocksManager), "GetSongState")]
    [HarmonyPrefix]
    static bool GetDifficultyUnlockPatch(ref bool __result, string songName, string selectedDifficulty)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        string songname2 = songName.ToLower();
        string diffname2 = selectedDifficulty.ToLower();
        foreach(string difficulty in DifficultyList.GetDifficulties())
        {
            if(difficulty.ToLower() == $"{songname2}/{diffname2}")
            {
                __result = true;
                return true;
            }
        }
        __result = false;
        return false;
    }
}