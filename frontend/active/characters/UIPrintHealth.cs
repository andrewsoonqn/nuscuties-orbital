using Godot;
using nuscutiesapp.tools;
using System;

public partial class UIPrintHealth : PrintHealth
{
    public Character TargetCharacter;
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        string hp = TargetCharacter.GetHP().ToString();
        Text = $"{hp} HP";
    }
}