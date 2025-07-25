using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using DirAccess = Godot.DirAccess;
using FileAccess = Godot.FileAccess;

public partial class QuestLogManager : Node
{
    private string GetSavePath()
    {
        var userManager = GetNode<UserManager>("/root/UserManager");
        string username = userManager?.GetCurrentUser() ?? "DefaultUser";
        return $"user://{username}_quest_saves/quest_log.json";
    }

    private string GetSaveDirectory()
    {
        var userManager = GetNode<UserManager>("/root/UserManager");
        string username = userManager?.GetCurrentUser() ?? "DefaultUser";
        return $"user://{username}_quest_saves";
    }

    public void SaveQuestLog(List<Quest> quests)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(quests, options);
        string saveDirectory = GetSaveDirectory();
        string savePath = GetSavePath();

        var dir = DirAccess.Open(saveDirectory);
        if (dir == null)
        {
            DirAccess.MakeDirRecursiveAbsolute(saveDirectory);
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
        string savePath = GetSavePath();

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
