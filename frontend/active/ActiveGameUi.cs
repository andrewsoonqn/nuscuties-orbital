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
    [Export] private DrawHealth _healthBar;
    [Export] private UIPrintHealth _healthLabel;
    private Node2D _gameWorld;

    public override void _Ready()
    {
        this.ProcessMode = ProcessModeEnum.Always;
        _pauseLabel.Show();
        _pauseMenu.Hide();
        _gameWorld = this.GetParent<Node>().GetNode<Node2D>("GameWorld");
        _resumeButton.Pressed += ResumeButtonOnPressed;
        _quitButton.Pressed += QuitButtonOnPressed;
        _healthBar.TargetCharacter = _gameWorld.GetNode<Character>("Player");
        _healthLabel.TargetCharacter = _gameWorld.GetNode<Character>("Player");
    }

    private void QuitButtonOnPressed()
    {
        _gameWorld.GetTree().SetPause(false);
        this.GetTree().ChangeSceneToFile(Paths.Active);
    }

    private void ResumeButtonOnPressed()
    {
        _gameWorld.GetTree().SetPause(false);
        // _pauseLabel.Show();
        _pauseMenu.Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEvent)
        {
            if (inputEvent.Keycode == Key.Escape && inputEvent.IsPressed())
            {
                _gameWorld.GetTree().SetPause(true);
                // _pauseLabel.Hide();
                _pauseMenu.Show();
            }
        }
    }
}