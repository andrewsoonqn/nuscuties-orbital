using Godot;
using System;

public partial class GoToDaily : Button
{
    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetTree().ChangeSceneToFile("res://daily/daily.tscn");
    }
}