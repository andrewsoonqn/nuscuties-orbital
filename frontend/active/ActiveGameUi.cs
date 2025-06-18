using Godot;
using nuscutiesapp.tools;
using System;
using System.Diagnostics;

public partial class ActiveGameUi : Control
{
    [Export] private PanelContainer _pauseLabel;
    [Export] private PanelContainer _pauseMenu;
    [Export] private Button _resumeButton;
    [Export] private Button _quitButton;
    private Node2D _gameWorld;

    public override void _Process(double delta)
    {
        GD.Print("ui running");
    }

    public override void _Ready()
    {
        this.ProcessMode = ProcessModeEnum.Always;
        _pauseLabel.Show();
        _pauseMenu.Hide();
        _gameWorld = this.GetParent<Node>().GetNode<Node2D>("GameWorld");
        _resumeButton.Pressed += ResumeButtonOnPressed;
        _quitButton.Pressed += QuitButtonOnPressed;
    }

    private void QuitButtonOnPressed()
    {
        _gameWorld.GetTree().SetPause(false);
        this.GetTree().ChangeSceneToFile(Paths.Active);
    }

    private void ResumeButtonOnPressed()
    {
        GD.Print("VARresume");
        _gameWorld.GetTree().SetPause(false);
        _pauseLabel.Show();
        _pauseMenu.Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEvent)
        {
            if (inputEvent.Keycode == Key.Escape && inputEvent.IsPressed())
            {

                _gameWorld.GetTree().SetPause(true);
                GD.Print("PRESSED");
                _pauseLabel.Hide();
                _pauseMenu.Show();
            }
        }
    }
}