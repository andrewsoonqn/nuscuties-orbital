using Godot;
using nuscutiesapp.tools;
using System;

public partial class Home : Control
{
    [Export] private Label _expLabel;
    [Export] private Button _dailyButton;
    [Export] private Button _passiveButton;
    [Export] private Button _activeButton;

    private ExpManager _expManager;

    public override void _Ready()
    {
        _expManager = GetNode<ExpManager>("/root/ExpManager");
        int exp = _expManager.GetExp();
        _expLabel.Text = $"EXP: {exp.ToString()}";

        _dailyButton.Pressed += () => switchScene(Paths.Daily);
        _passiveButton.Pressed += () => switchScene(Paths.Passive);
        _activeButton.Pressed += () => switchScene(Paths.Active);

    }

    public void switchScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}