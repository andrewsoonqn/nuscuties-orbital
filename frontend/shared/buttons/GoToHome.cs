using Godot;
using System;

public partial class GoToHome : Button
{
    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetTree().ChangeSceneToFile("res://shared/Home.tscn");
    }
}