using Godot;
using System;

public partial class Character : CharacterBody2D
{
    public const float Friction = 0.15f;

    [Export] private float _acceleration = 0.30f;
    [Export] private float _maxSpeed = 50;

    protected AnimatedSprite2D AnimatedSprite;

    protected Vector2 MovDirection = Vector2.Zero;

    public override void _Ready()
    {
        this.AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
    }

    public void Move()
    {
        MovDirection = MovDirection.Normalized();
        GD.Print(MovDirection*_maxSpeed);
        Velocity = Velocity.Lerp(MovDirection * _maxSpeed, _acceleration);
        // Velocity += MovDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }

    public virtual void GetInput()
    { }
}