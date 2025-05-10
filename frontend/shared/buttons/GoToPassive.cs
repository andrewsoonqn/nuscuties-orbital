using Godot;
using System;

public partial class GoToPassive : Button
{
    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetTree().ChangeSceneToFile("res://passive/passive.tscn");
    }
}