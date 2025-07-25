using Godot;
using System;
using nuscutiesapp.tools;

public partial class Settings : Control
{
	[Export] private Button _tutorialButton;
	[Export] private Button _soundOptionsButton;
	[Export] private Button _backToHomeButton;
	
	public override void _Ready()
	{
		_soundOptionsButton.Pressed += () => switchScene(Paths.SoundOptions);
		_backToHomeButton.Pressed += () => switchScene(Paths.Home);
		_tutorialButton.Pressed += () => switchScene(Paths.Tutorial);
	}
	
	public void switchScene(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}
}
