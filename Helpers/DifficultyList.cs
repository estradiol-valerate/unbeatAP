using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace UNBEATAP.Helpers;

public static class DifficultyList
{
    private static List<string> diffs = new List<string>();
    
    private static List<string> difficulties = new List<string>();


    public static void AddDifficulty(string song, string difficulty)
    {
        difficulties.Add($"{song}/{difficulty}");
    }


    public static List<string> GetDifficulties()
    {
        return difficulties;
    }
    

    public static List<string> Init(string songName)
    {
        diffs.Clear();
        // This expects the mod to be in UNBEATABLE/BepInEx/plugins/unbeatAP, and the Assets folder with difficulties.json to be next to it.
        string json = File.ReadAllText("BepInEx/plugins/unbeatAP/Assets/difficulties.json");
        JObject data = JObject.Parse(json);
        foreach(JToken diff in data[songName])
        {
            diffs.Add((string)diff);
        }
        return diffs;
    }
}