using Godot;
using Microsoft.VisualBasic.CompilerServices;
using nuscutiesapp.tools;
using System;
using System.IO;
using System.Text.Json;

public partial class PlayerStatManager : BaseStatManager<PlayerStatManager.PlayerStatData>
{
    private ProgressionManager _progressionManager;
    public override void _Ready()
    {
        _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        _progressionManager.LeveledUp += ProgressionManagerOnLeveledUp;
        base._Ready();
    }

    private void ProgressionManagerOnLeveledUp(int level)
    {
        AddStatPoints(5);
    }

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
    [Signal]
    public delegate void StrengthChangedEventHandler(int strength);

    [Signal]
    public delegate void StaminaChangedEventHandler(int stamina);

    private void AddStatPoints(int value)
    {
        Data.TotalStatPoints += value;
        Data.RemainingStatPoints += value;
        NotifyDataChanged();
    }
    public void AddStrength(int value)
    {
        if (Data.RemainingStatPoints <= 0 && value >= 0) return;
        Data.Strength += value;
        Data.RemainingStatPoints -= value;
        NotifyDataChanged();
        EmitSignal(nameof(StrengthChanged), Data.Strength);
    }

    public void AddStamina(int value)
    {
        if (Data.RemainingStatPoints <= 0 && value >= 0) return;
        Data.Stamina += value;
        Data.RemainingStatPoints -= value;
        NotifyDataChanged();
        EmitSignal(nameof(StaminaChanged), Data.Stamina);
    }

    public int GetRemainingStatPoints()
    {
        return Data.RemainingStatPoints;
    }

    public int GetStrength()
    {
        return Data.Strength;
    }

    public int GetStamina()
    {
        return Data.Stamina;
    }
}
