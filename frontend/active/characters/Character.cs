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
        GD.Print(Name, "physics");
        MoveAndSlide();
        GD.Print(Name, Velocity, "before");
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
        GD.Print(Name, Velocity, "after");
    }

    public void Move()
    {
        GD.Print(this.Name, "move", MovDirection, Velocity);
        MovDirection = MovDirection.Normalized();
        Velocity = Velocity.Lerp(MovDirection * _maxSpeed, _acceleration);
        // Velocity += MovDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }

    public virtual void GetInput()
    { }
}