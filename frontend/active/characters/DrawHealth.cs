using Godot;
using System;

public partial class DrawHealth : ProgressBar
{
    [Export] public Character TargetCharacter;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        this.MaxValue = TargetCharacter.GetMaxHP();
        this.Value = TargetCharacter.GetHP();
    }
}