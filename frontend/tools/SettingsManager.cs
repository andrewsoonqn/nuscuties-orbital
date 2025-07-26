using Godot;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class SettingsManager : Node
{
    private AudioManager _audioManager;
    public class SettingsData
    {
        public float BgmVolume { get; set; } = 1f;
        public float SfxVolume { get; set; } = 1f;
    }

    public override void _Ready()
    {
        _audioManager = GetNode<AudioManager>("/root/AudioManager");
        base._Ready();
    }

    private string GetSavePath()
    {
        return "user://saves/settings.json";
    }
    private string GetSaveDirectory()
    {
        return "user://saves";
    }

    public void SaveSettings(SettingsData settings)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(settings);
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
            GD.PrintErr("Error saving settings. File is null.");
        }
    }

    public SettingsData LoadSettings()
    {
        string savePath = GetSavePath();

        SettingsData res = new SettingsData();
        if (FileAccess.FileExists(savePath))
        {
            using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            if (file != null)
            {
                string jsonString = file.GetAsText();
                try
                {
                    SettingsData loadedLog = JsonSerializer.Deserialize<SettingsData>(jsonString);
                    res = loadedLog;
                }
                catch (System.Exception e)
                {
                    GD.PrintErr($"Error loading settings: {e.Message}. Starting new log.");
                }
            }
            else
            {
                GD.PrintErr("Error opening settings file. File is null.");
            }
        }
        else
        {
            GD.PrintErr("No settings file found. Starting new log.");
        }

        return res;
    }
}