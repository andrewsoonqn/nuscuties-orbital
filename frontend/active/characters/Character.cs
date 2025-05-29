using Godot;
using System;

public partial class Character : CharacterBody2D
{
    public const float Friction = 0.15f;

    [Export] private int _acceleration = 250;
    [Export] private float _maxSpeed = 20000;

    protected AnimatedSprite2D _animatedSprite;

    protected Vector2 _movDirection = Vector2.Zero;

    public override void _Ready()
    {
        this._animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
    }

    public void Move()
    {
        _movDirection = _movDirection.Normalized();
        Velocity = _movDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }

    public virtual void GetInput()
    { }
}