using Godot;
using nuscutiesapp.tools;
using System;

public partial class Home : Control
{
    [Export] private Label _expLabel;
    [Export] private Label _levelLabel;
    [Export] private Label _remainingExpLabel;
    [Export] private Button _dailyButton;
    [Export] private Button _passiveButton;
    [Export] private Button _activeButton;
    [Export] private Button _statsUIButton;

    private ProgressionManager _expManager;

    private PackedScene _statsUINode;
    public override void _Ready()
    {
        _expManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        int exp = _expManager.GetExp();
        int level = _expManager.GetLevel();
        int totalExpNeeded = ProgressionManager.GetTotalExpRequiredForLevel(level + 1);
        _levelLabel.Text = $"Level {level}";
        _expLabel.Text = $"EXP: {exp} / {totalExpNeeded}";
        _remainingExpLabel.Text = $"{totalExpNeeded - exp} EXP to next level";

        _dailyButton.Pressed += () => switchScene(Paths.Daily);
        _passiveButton.Pressed += () => switchScene(Paths.Passive);
        _activeButton.Pressed += () => switchScene(Paths.Active);

        _statsUINode = ResourceLoader.Load<PackedScene>(Paths.StatsUI);
        
        _statsUIButton.Pressed += StatsUIButtonOnPressed;
    }

    private void StatsUIButtonOnPressed()
    {
        if (!GetTree().Root.HasNode("StatUserInterface"))
        {
            GetTree().Root.AddChild(_statsUINode.Instantiate());
        }
    }

    public void switchScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}