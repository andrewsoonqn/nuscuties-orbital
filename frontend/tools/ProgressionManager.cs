using Godot;
using Microsoft.VisualBasic.CompilerServices;
using nuscutiesapp.tools;
using System;
using System.IO;
using System.Text.Json;

public partial class ProgressionManager : BaseStatManager<ProgressionManager.ProgressionData>
{
    public override void _Ready()
    {
        LeveledUp += OnLeveledUp;
        base._Ready();
    }

    private void OnLeveledUp(int level, int extraLevels)
    {
        // PackedScene statsUI = ResourceLoader.Load<PackedScene>(Paths.StatsUI);
        // GetTree().Root.AddChild(statsUI.Instantiate());
    }

    public static readonly int BaseExp = 300;
    public static readonly double ScaleFactor = 1.2;
    public class ProgressionData
    {
        public int Exp { get; set; } = 0;
        public int Level { get; set; } = 1;
    }

    protected override string BaseSaveFileName => "player_progression.json";

    public void SetSaveFilePath(string newFilePath)
    {
        // Should be used for testing only! - Note: This method is now obsolete with user-prefixed paths
        GD.PrintErr("SetSaveFilePath is obsolete with user-prefixed save system");
    }

    protected override void InitializeDefaults()
    {
        int calculatedLevel = CalculateLevel(Data.Exp);
        if (Data.Level != calculatedLevel)
        {
            Data.Level = calculatedLevel;
            NotifyDataChanged();
        }
        base.InitializeDefaults();
    }

    protected override void OnDataChanged()
    {
        // No need to do anything here, save handled by base class
    }

    // Public API
    [Signal]
    public delegate void LeveledUpEventHandler(int level, int extraLevels);

    public int GetExp()
    {
        return Data.Exp;
    }

    public int GetLevel()
    {
        return Data.Level;
    }

    public void AddExp(int exp)
    {
        Data.Exp += exp;
        Data.Exp = Math.Max(Data.Exp, 0);

        int newLevel = CalculateLevel(Data.Exp);
        if (newLevel != Data.Level)
        {
            if (newLevel > Data.Level)
            {
                EmitSignal(nameof(LeveledUp), newLevel, newLevel - Data.Level);
            }
            Data.Level = newLevel;
        }

        NotifyDataChanged();
    }

    // Level calculation functions
    private static int CalculateLevel(int totalExp)
    {
        if (totalExp < BaseExp) return 1;

        // Exp for level L = BaseExp * (ScaleFactor^L - 1) / (ScaleFactor - 1)
        double level = Math.Log(1 + totalExp * (ScaleFactor - 1) / BaseExp) / Math.Log(ScaleFactor) + 1;
        level = Math.Round(level, 2);
        return (int)Math.Floor(level);
    }

    public static int GetTotalExpRequiredForLevel(int level)
    {
        if (level <= 1) return 0;

        double totalExp = BaseExp * (Math.Pow(ScaleFactor, level - 1) - 1) / (ScaleFactor - 1);
        return (int)Math.Round(totalExp);
    }

    public static int GetExpRequiredForLevel(int level)
    {
        if (level <= 1) return BaseExp;

        return GetTotalExpRequiredForLevel(level) - GetTotalExpRequiredForLevel(level - 1);
    }

}