using Godot;
using System;

public partial class PrintState : Label
{
    private Character _owner;

    public override void _Ready()
    {
        _owner = GetParent<Character>();
    }

    public override void _PhysicsProcess(double delta)
    {
        this.Text = _owner.GetMovementState().ToString() + "\n" + _owner.GetActionState().ToString();
    }
}