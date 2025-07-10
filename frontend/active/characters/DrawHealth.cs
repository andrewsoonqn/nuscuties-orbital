using Godot;
using System;

public partial class DrawHealth : TextureProgressBar
{
    [Export] public Character TargetCharacter;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print(MaxValue);
        GD.Print(Value);
        this.MaxValue = TargetCharacter.GetMaxHP();
        this.Value = TargetCharacter.GetHP();
    }
}