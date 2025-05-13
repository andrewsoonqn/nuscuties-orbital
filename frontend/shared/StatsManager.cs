using Godot;
using System;
using System.Text.Json;
using System.IO;

public partial class StatsManager : Node
{
    private int Exp { get; set; } = 0;

    private const string SaveFilePath = "user://stats.json";

    public override void _Ready()
    {
        LoadStats();
        GD.Print($"StatsManager ready. Loaded EXP: {Exp}");
    }

    public int GetExp()
    {
        return Exp;
    }
    
    public void AddExp(int amount)
    {
        if (amount > 0)
        {
            Exp += amount;
            GD.Print($"Gained {amount} EXP. Current EXP: {Exp}");
            SaveStats();
        }
    }

    public void ResetExp()
    {
        Exp = 0;
        SaveStats();
    } 
    public class PlayerStats
    {
        public int Exp { get; set; }
    }

    public void SaveStats()
    {
        var data = new PlayerStats() { Exp = this.Exp, };

        var options = new JsonSerializerOptions { WriteIndented = true }; // For human-readable format
        string json_string = JsonSerializer.Serialize(data, options);

        try
        {
            var dir_path = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(dir_path))
            {
                Directory.CreateDirectory(dir_path);
            }

            File.WriteAllText(SaveFilePath, json_string);
            GD.Print($"EXP saved to {SaveFilePath}");
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error saving EXP: {e.Message}");
        }
    }

    public void LoadStats()
    {
        try
        {
            string jsonString = File.ReadAllText(SaveFilePath);
            var loadedData = JsonSerializer.Deserialize<PlayerStats>(jsonString);

            if (loadedData != null)
            {
                Exp = loadedData.Exp;
                GD.Print($"EXP loaded from {SaveFilePath}");
            }
            else
            {
                GD.PrintErr("Error deserializing EXP file. Data might be corrupt.");
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading EXP file: {e.Message}");
        }
    }

    // Save EXP automatically when game window closed
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            SaveStats();
        }
    }
}