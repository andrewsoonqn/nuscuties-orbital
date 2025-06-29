using Godot;
using Microsoft.VisualBasic.CompilerServices;
using nuscutiesapp.tools;
using System;
using System.IO;
using System.Text.Json;

public partial class PlayerStatManager : BaseStatManager<PlayerStatManager.PlayerStatData>
{
    public class PlayerStatData
    {
        public int RemainingStatPoints { get; set; } = 0;
        public int TotalStatPoints { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Stamina { get; set; } = 0;
    }

    private string _saveFilePath = "user://player_stats.json";
    protected override string SaveFilePath => _saveFilePath;

    public void SetSaveFilePath(string newFilePath)
    {
        // Should be used for testing only!
        _saveFilePath = newFilePath;
    }

    protected override void OnDataChanged()
    {
        // No need to do anything here, save handled by base class
    }

    // Public API
    public void AddStatPoints(int value)
    {
        Data.TotalStatPoints += value;
        Data.RemainingStatPoints += value;
        NotifyDataChanged();
    }
    public void AddStrength(int value)
    {
        if (Data.RemainingStatPoints <= 0) return;
        Data.Strength += value;
        Data.RemainingStatPoints -= value;
        NotifyDataChanged();
    }

    public void AddAgility(int value)
    {
        if (Data.RemainingStatPoints <= 0) return;
        Data.Agility += value;
        Data.RemainingStatPoints -= value;
        NotifyDataChanged();
    }

    public void AddStamina(int value)
    {
        if (Data.RemainingStatPoints <= 0) return;
        Data.Stamina += value;
        Data.RemainingStatPoints -= value;
        NotifyDataChanged();
    }
}