using Godot;
using System;

public partial class ActiveGame : Node2D
{
    public override void _Ready()
    {
        var camera = new FollowTargetCamera();
        camera.Name = "FollowPlayerCamera";
        camera.Target = this.GetNode<CharacterBody2D>("Player");
        camera.Zoom = new Vector2(10, 10);
        this.AddChild(camera);

        base._Ready();
    }
}