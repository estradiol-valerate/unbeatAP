using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UNBEATAP.Helpers;

public static class DifficultyList
{
    public const string NameSeparator = "/";

    public static readonly Dictionary<string, string> LowerToFullDiffName = new Dictionary<string, string>
    {
        {"beginner", "Beginner"},
        {"easy", "Easy"},
        {"normal", "Normal"},
        {"hard", "Hard"},
        {"unbeatable", "UNBEATABLE"},
        {"star", "Star"}
    };

    private static Dictionary<string, string[]> diffDict;
    private static Dictionary<string, string> songDict;

    private static List<string> currentDifficulties = new List<string>();
    private static List<string> visibleDifficulties = new List<string>();


    public static List<string> GetDifficulties()
    {
        return currentDifficulties;
    }


    public static List<string> GetVisibleDifficulties()
    {
        return visibleDifficulties;
    }


    private static void SortVisibleDifficulties()
    {
        // We need to do this because the difficulty switcher assumes they're sorted properly
        List<string> sortedDifficulties = new List<string>();

        // The dictionary has them listed in order, so we can just check each one in order and get a sorted list
        foreach(KeyValuePair<string, string> pair in LowerToFullDiffName)
        {
            if(visibleDifficulties.Contains(pair.Value))
            {
                sortedDifficulties.Add(pair.Value);
            }
        }

        visibleDifficulties = sortedDifficulties;
    }


    private static void AddDifficulty(string song, string difficulty)
    {
        currentDifficulties.Add($"{song}{NameSeparator}{difficulty}");

        string newDiffName = LowerToFullDiffName[difficulty.ToLower()];
        if(!visibleDifficulties.Contains(newDiffName))
        {
            visibleDifficulties.Add(newDiffName);
            SortVisibleDifficulties();
        }
    }


    private static string GetInternalName(string songName)
    {
        if(songDict == null)
        {
            return null;
        }

        return songDict[songName].ToLower();
    }


    public static bool TryAddSongItem(string songName, int diffIndex)
    {
        string internalName = GetInternalName(songName);
        
        if(!diffDict.TryGetValue(internalName, out string[] difficulties))
        {
            Plugin.Logger.LogWarning($"Failed to get difficulties for song: {internalName}");
            return false;
        }

        if(difficulties.Length <= diffIndex)
        {
            int extraDiffs = diffIndex - difficulties.Length + 1;
            Plugin.Logger.LogWarning($"Received {extraDiffs} extra difficult(y)(ies) for song: {internalName}");
            return false;
        }

        string newDifficulty = difficulties[diffIndex];
        AddDifficulty(internalName, newDifficulty);
        return true;
    }


    private static async Task LoadDifficulties()
    {
        const string diffFile = "difficulties.json";

        string diffPath = Path.Combine(Plugin.AssetsFolder, diffFile);
        string json = await File.ReadAllTextAsync(diffPath);
        diffDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
    }


    private static async Task LoadSongs()
    {
        const string songFile = "songs.json";

        string songPath = Path.Combine(Plugin.AssetsFolder, songFile);
        string json = await File.ReadAllTextAsync(songPath);
        songDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }
    

    public static async Task Init()
    {
        try
        {
            await LoadDifficulties();
            await LoadSongs();
        }
        catch(Exception e)
        {
            Plugin.Logger.LogError($"Failed to load assets with error: {e.Message}, {e.StackTrace}");
        }
    }
}