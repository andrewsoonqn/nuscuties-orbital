using Godot;
using nuscutiesapp.tools;
using System;

public partial class Home : Control
{
    [Export] private Label _expLabel;
    [Export] private Label _levelLabel;
    [Export] private ProgressBar _expProgressBar;
    [Export] private Button _dailyButton;
    [Export] private Button _passiveButton;
    [Export] private Button _activeButton;
    [Export] private Button _statsUIButton;
    [Export] private Button _storeButton;

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
        _expProgressBar.MinValue = 0;
        _expProgressBar.MaxValue = totalExpNeeded;
        _expProgressBar.Value = exp;

        _dailyButton.Pressed += () => switchScene(Paths.Daily);
        _passiveButton.Pressed += () => switchScene(Paths.Passive);
        _activeButton.Pressed += () => switchScene(Paths.Active);
        _storeButton.Pressed += () => switchScene(Paths.Shop);

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
