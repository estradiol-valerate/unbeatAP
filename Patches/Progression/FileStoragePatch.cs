using HarmonyLib;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

[HarmonyPatch(typeof(FileStorage))]
public class FileStoragePatch
{
    [HarmonyPatch("highscores", MethodType.Getter)]
    [HarmonyPrefix]
    static bool HighscoresPrefix(ref HighScoreList __result)
    {
        if(!Plugin.Client.Connected)
        {
            return true;
        }

        // Use our custom archipelago high score implementation
        __result = HighScoreHandler.HighScores;
        return false;
    }
}