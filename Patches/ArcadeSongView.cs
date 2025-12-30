using HarmonyLib;
using Arcade.Progression;
using Rhythm;
using UNBEATAP.Helpers;

namespace UNBEATAP.Patches;

public class ArcadeSongView
{
    // This patch hides all the songs from the arcade list unless you have set in SongList
    [HarmonyPatch(typeof(SongUnlocksManager), "GetSongState")]
    [HarmonyPrefix]
    static bool GetSongStatePatch(ref bool __result, BeatmapIndex.Song song)
    {
        foreach(string unlock in SongList.GetSongs())
        {
            if(song.name.ToLower() == unlock)
            {
                __result = true;
                return true;
            }
        }
        
        __result = false;
        return false;
    }
}
