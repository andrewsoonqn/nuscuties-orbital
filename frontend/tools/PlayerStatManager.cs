using Godot;
using Microsoft.VisualBasic.CompilerServices;
using nuscutiesapp.tools;
using System;
using System.IO;
using System.Text.Json;

public partial class PlayerStatManager : BaseStatManager<PlayerStatManager.PlayerStatData>
{
    private readonly int _pointsPerLevel = 5;
    private ProgressionManager _progressionManager;
    public override void _Ready()
    {
        _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        _progressionManager.LeveledUp += ProgressionManagerOnLeveledUp;
        base._Ready();
        Data.TotalStatPoints = (_progressionManager.GetLevel() - 1) * _pointsPerLevel;
        EmitSignalStatPointsChanged(Data.TotalStatPoints);
    }

    private void ProgressionManagerOnLeveledUp(int level, int extraLevels)
    {
        Data.TotalStatPoints = (level - 1) * _pointsPerLevel;
        EmitSignal(nameof(StatPointsChanged), Data.TotalStatPoints);
    }

    public class PlayerStatData
    {
        public int TotalStatPoints { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Stamina { get; set; } = 0;
    }

    protected override string BaseSaveFileName => "player_stats.json";

    // TODO: add check to make sure all numbers add up correctly

    public void SetSaveFilePath(string newFilePath)
    {
        // Should be used for testing only! - Note: This method is now obsolete with user-prefixed paths
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

    [Signal]
    public delegate void StatPointsChangedEventHandler(int statPoints);

    public void AddStrength(int value)
    {
        if (GetRemainingStatPoints() <= 0 && value >= 0) return;
        if (Data.Strength == 0 && value < 0) return;
        Data.Strength += value;
        NotifyDataChanged();
        EmitSignal(nameof(StrengthChanged), Data.Strength);
    }

    public void AddStamina(int value)
    {
        if (GetRemainingStatPoints() <= 0 && value >= 0) return;
        if (Data.Stamina == 0 && value < 0) return;
        Data.Stamina += value;
        NotifyDataChanged();
        EmitSignal(nameof(StaminaChanged), Data.Stamina);
    }

    public int GetRemainingStatPoints()
    {
        return Data.TotalStatPoints - Data.Stamina - Data.Strength;
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
