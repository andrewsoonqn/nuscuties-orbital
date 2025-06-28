using Godot;
using Microsoft.VisualBasic.CompilerServices;
using nuscutiesapp.tools;
using System;
using System.IO;
using System.Text.Json;

public partial class ProgressionManager : BaseStatManager<ProgressionManager.ProgressionData>
{
    public static readonly int BaseExp = 300;
    public static readonly double ScaleFactor = 1.2;
    public class ProgressionData
    {
        public int Exp { get; set; } = 0;
        public int Level { get; set; } = 1;
    }

    private string _saveFilePath = "user://player_progression.json";
    protected override string SaveFilePath => _saveFilePath;

    public void SetSaveFilePath(string newFilePath)
    {
        // Should be used for testing only!
        _saveFilePath = newFilePath;
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
        // Signals handled by Godot signal logic
    }

    // Public API
    public delegate void LevelChangedEventHandler(int level);
    public delegate void ExpChangedEventHandler(int exp);

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

    private static int GetTotalExpRequiredForLevel(int level)
    {
        if (level <= 1) return 0;

        double totalExp = BaseExp * (Math.Pow(ScaleFactor, level - 1) - 1) / (ScaleFactor - 1);
        return (int)Math.Round(totalExp);
    }

    private static int GetExpRequiredForLevel(int level)
    {
        if (level <= 1) return BaseExp;

        return GetTotalExpRequiredForLevel(level) - GetTotalExpRequiredForLevel(level - 1);
    }

}