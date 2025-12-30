using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace UNBEATAP.Helpers
{
    public static class SongList
    {
        // If using the ArcadeSongView patch, this will hide every song except for what is in the list.
        private static List<string> songs = new List<string>();

        
        public static void AddSong(string song)
        {
            songs.Add(song.ToLower());
        }


        public static List<string> GetSongs()
        {
            return songs;
        }
        

        // May not be needed, depending on how we handle getting score data after song completion
        public static string SongNameToUnlockName(string songName)
        {
            songName = songName.ToLower();
            // This expects the mod to be in UNBEATABLE/BepInEx/plugins/unbeatAP, and the Assets folder with songs.json to be next to it.
            string json = File.ReadAllText("BepInEx/plugins/unbeatAP/Assets/songs.json");
            JObject data = JObject.Parse(json);
            string unlockName = data[songName].ToString();
            return unlockName;
        }
    }
}