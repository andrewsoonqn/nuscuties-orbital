using Godot;
using System;

public partial class Character : CharacterBody2D
{
    public const float Friction = 0.15f;
    
    [Export] private int _acceleration = 40;
    [Export] private float _maxSpeed = 100;
    
    private Vector2 _movDirection = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
    }

    private void Move()
    {
        _movDirection = _movDirection.Normalized();
        Velocity = _movDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }
}