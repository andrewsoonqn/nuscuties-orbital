using Godot;
using System;
using nuscutiesapp.tools;

public partial class Tutorial : Control
{
	[Export] private Button _backToSettingsButton;
	
	public override void _Ready()
	{
		_backToSettingsButton.Pressed += () => switchScene(Paths.Settings);
	}
	
	public void switchScene(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}
}
