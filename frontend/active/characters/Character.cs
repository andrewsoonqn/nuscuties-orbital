using Godot;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices.JavaScript;

public abstract partial class Character : CharacterBody2D
{
    public const float Friction = 0.15f;

    [Export] private float _acceleration = 0.30f;
    [Export] private float _maxSpeed = 50;

    public AnimatedSprite2D AnimatedSprite;

    public Vector2 MovDirection = Vector2.Zero;

    protected IMovementStrategy MovementStrategy;

    protected StateMachine<IMovementState> MovementStateMachine;
    protected StateMachine<IActionState> ActionStateMachine;

    public override void _Ready()
    {
        this.AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        _runStateMachines(delta);
        MoveAndSlide();
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
    }

    private void _runStateMachines(double delta)
    {
        MovementStateMachine.Update(delta);
        ActionStateMachine.Update(delta);
    }

    public void Move()
    {
        MovDirection = MovDirection.Normalized();
        Velocity = Velocity.Lerp(MovDirection * _maxSpeed, _acceleration);
        // Velocity += MovDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }

    public void GetDirection()
    {
        this.MovementStrategy.GetDirection();
    }

    // Allow state classes to change the current movement state while still keeping the
    // state machine encapsulated within the Character class.
    public void ChangeMovementState(IMovementState newState)
    {
        MovementStateMachine?.SetState(newState);
    }

    public abstract void PlayIdleAnimation();
    public abstract void PlayMoveAnimation();
}