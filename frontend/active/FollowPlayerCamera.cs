using Godot;
using System;

public partial class FollowPlayerCamera : Camera2D
{
    [Export]
    public Node2D Target { get; set; }

    public override void _Process(double delta)
    {
        if (Target != null)
        {
            this.GlobalPosition = Target.GlobalPosition;
        }
    }
}