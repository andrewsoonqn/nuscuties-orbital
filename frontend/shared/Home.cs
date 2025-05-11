using Godot;
using System;

public partial class Home : Control
{
    [Export] private Label _expLabel;
    [Export] private Button _dailyButton;
    [Export] private Button _passiveButton;
    [Export] private Button _activeButton;

    private StatsManager _statsManager;
    private const string _dailyPath = "res://daily/daily.tscn";
    private const string _passivePath = "res://passive/passive.tscn";
    private const string _activePath = "res://active/active.tscn";
 
    public override void _Ready()
    {
        _statsManager = GetNode<StatsManager>("/root/StatsManager");
        int exp = _statsManager.GetExp();
        GD.Print($"Main scene got {exp}");
        _expLabel.Text = $"EXP: {exp.ToString()}";
        
        _dailyButton.Pressed += () => switchScene(_dailyPath);
        _passiveButton.Pressed += () => switchScene(_passivePath);
        _activeButton.Pressed += () => switchScene(_activePath);
        
    }
    
    public void switchScene(string scenePath)
    {
        GD.Print($"Switching scene to {scenePath}");
        GetTree().ChangeSceneToFile(scenePath);
    }
}