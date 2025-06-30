using Godot;
using System;

public partial class PrintHealth : Label
{
    protected Character _owner;

    public override void _Ready()
    {
        _owner = GetParent<Character>();
    }

    public override void _PhysicsProcess(double delta)
    {
        this.Text = _owner.GetHP().ToString();
    }
}