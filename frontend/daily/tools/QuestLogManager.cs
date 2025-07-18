using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using DirAccess = Godot.DirAccess;
using FileAccess = Godot.FileAccess;

public partial class QuestLogManager : Node
{
    public string savePath = "user://quest_saves/quest_log.json";

    public void SaveQuestLog(List<Quest> quests)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(quests, options);

        var dir = DirAccess.Open("user://quest_saves");
        if (dir == null)
        {
            DirAccess.MakeDirRecursiveAbsolute("user://quest_saves");
        }

        using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        if (file != null)
        {
            file.StoreString(jsonString);
        }
        else
        {
            GD.PrintErr("Error saving quest log. File is null.");
        }
    }

    public List<Quest> LoadQuestLog()
    {
        List<Quest> quests = new List<Quest>();
        if (FileAccess.FileExists(savePath))
        {
            using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            if (file != null)
            {
                string jsonString = file.GetAsText();
                try
                {
                    List<Quest> loadedLog = JsonSerializer.Deserialize<List<Quest>>(jsonString);
                    quests = loadedLog ?? new List<Quest>();
                }
                catch (System.Exception e)
                {
                    GD.PrintErr($"Error loading quest log: {e.Message}. Starting new log.");
                    quests = new List<Quest>();
                }
            }
            else
            {
                GD.PrintErr("Error opening quest log file. File is null.");
            }
        }
        else
        {
            GD.PrintErr("No quest log file found. Starting new log.");
        }

        return quests;
    }
}