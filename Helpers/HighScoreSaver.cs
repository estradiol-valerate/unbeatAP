using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Enums;
using Newtonsoft.Json;

namespace UNBEATAP.Helpers;

public static class HighScoreSaver
{
    public const string HighScoresKey = "highscores";

    public static readonly string[] NoteJudgements = new string[]
    {
        "Critical",
        "Perfect",
        "Great",
        "Good",
        "Ok",
        "Barely",
        "Miss"
    };


    public static byte[] SerializeHighscore(HighScoreItem item)
    {
        item.OnBeforeSerialize();

        using MemoryStream stream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(stream);

        writer.Write(item.song);
        writer.Write(item.score);
        writer.Write(item.accuracy);
        writer.Write(item.maxCombo);
        writer.Write(item.cleared);
        writer.Write((int)item.modifierMask);
        writer.Write(item.level);

        Dictionary<string, int> notes = item._notes;
        // Note counts are always ordered in descending accuracy
        foreach(int count in notes.Values)
        {
            writer.Write(count);
        }

        return stream.ToArray();
    }


    public static HighScoreItem DeserializeHighscore(byte[] highscore)
    {
        using MemoryStream byteStream = new MemoryStream(highscore);
        using BinaryReader reader = new BinaryReader(byteStream);

        string song = reader.ReadString();
        int score = reader.ReadInt32();
        float accuracy = reader.ReadSingle();
        int maxCombo = reader.ReadInt32();
        bool cleared = reader.ReadBoolean();
        int modifiers = reader.ReadInt32();
        int level = reader.ReadInt32();

        Dictionary<string, int> noteCounts = new Dictionary<string, int>();
        for(int i = 0; i < NoteJudgements.Length; i++)
        {
            string key = NoteJudgements[i];
            noteCounts[key] = reader.ReadInt32();
        }

        return new HighScoreItem(
            song,
            score,
            accuracy,
            maxCombo,
            cleared,
            noteCounts,
            (Modifiers)modifiers,
            level
        );
    }


    public static string SerializeHighscores(HighScoreList list)
    {
        list.OnBeforeSerialize();

        List<byte[]> scores = new List<byte[]>(list.highScores.Count);
        foreach(HighScoreItem highScore in list.highScores)
        {
            byte[] newScore = SerializeHighscore(highScore);
            if(newScore != null)
            {
                scores.Add(newScore);
            }
        }

        SerializedHighscoreList newList = new SerializedHighscoreList
        {
            scores = scores.ToArray()
        };

        return JsonConvert.SerializeObject(newList);
    }


    public static HighScoreList DeserializeHighscores(string serialized)
    {
        SerializedHighscoreList list = JsonConvert.DeserializeObject<SerializedHighscoreList>(serialized);

        List<HighScoreItem> highscores = new List<HighScoreItem>();
        foreach(byte[] serializedScore in list.scores)
        {
            highscores.Add(DeserializeHighscore(serializedScore));
        }

        HighScoreList newList = new HighScoreList();
        newList.highScores = highscores;
        newList.OnAfterDeserialize();

        return newList;
    }


    public static void SaveHighscores()
    {
        if(!Plugin.Client.Connected)
        {
            return;
        }

        try
        {
            string serializedScores = SerializeHighscores(HighScoreHandler.HighScores);
            Plugin.Logger.LogInfo($"Saving high scores with size: {(float)serializedScores.Length / 1000}kb");
            Plugin.Client.Session.DataStorage[Scope.Slot, HighScoresKey] = serializedScores;
        }
        catch(Exception e)
        {
            Plugin.Logger.LogError($"Failed to save highscores with error: {e.Message}, {e.StackTrace}");
        }
    }


    public static async Task LoadHighscores()
    {
        if(!Plugin.Client.Connected)
        {
            HighScoreHandler.HighScores = new HighScoreList();
            HighScoreHandler.ResetSavedRating();
            return;
        }

        try
        {
            string serializedScores = await Plugin.Client.Session.DataStorage[Scope.Slot, HighScoresKey].GetAsync<string>();
            if(string.IsNullOrEmpty(serializedScores))
            {
                Plugin.Logger.LogInfo($"No saved high scores yet, creating new.");
                HighScoreHandler.HighScores = new HighScoreList();
                HighScoreHandler.ResetSavedRating();
                return;
            }

            HighScoreList highScores = DeserializeHighscores(serializedScores);

            HighScoreHandler.HighScores = highScores;
            HighScoreHandler.ResetSavedRating();
        }
        catch(Exception e)
        {
            Plugin.Logger.LogError($"Failed to load high scores with error: {e.Message}, {e.StackTrace}");
        }
    }


    [Serializable]
    private class SerializedHighscoreList
    {
        public byte[][] scores;
    }
}