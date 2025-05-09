using Godot;
using System;

public partial class GoToActive : Button
{
    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetTree().ChangeSceneToFile("res://active/Active.tscn");
    }
}