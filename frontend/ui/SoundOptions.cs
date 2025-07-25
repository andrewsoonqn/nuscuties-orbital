using Godot;
using System;
using nuscutiesapp.tools;

public partial class SoundOptions : Control
{
	[Export] private Button _backToHomeButton;
	
	public override void _Ready()
	{
		_backToHomeButton.Pressed += () => switchScene(Paths.Settings);
	}
	
	public void switchScene(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}
}
