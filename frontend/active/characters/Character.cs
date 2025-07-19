using Godot;
using nuscutiesapp.active.characters.ActivateWeaponStrategies;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using nuscutiesapp.active.characters.Weapons;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

public abstract partial class Character : CharacterBody2D
{
    public const float Friction = 0.15f;

    [Export] private float _acceleration = 0.30f;
    [Export] private float _maxSpeed = 50;

    public AnimatedSprite2D AnimatedSprite;
    public AnimationPlayer MyAnimationPlayer;

    public Vector2 MovDirection = Vector2.Zero;

    protected IMovementStrategy MovementStrategy;

    protected StateMachine<IMovementState> MovementStateMachine;
    protected StateMachine<IActionState> ActionStateMachine;

    public HealthComponent Health;

    private ActiveDungeonEventManager _eventManager;

    protected Weapon MyWeapon;
    protected IActivateWeaponStrategy ActivateWeaponStrategy;

    public override void _Ready()
    {
        this._eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        this.AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        this.MyAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        this.Health = GetNode<HealthComponent>("Health");

        Health.Damaged += OnDamaged;
        Health.Died += OnDied;

        this.PlayIdleAnimation();
        this.Visible = true;
    }

    public void OnDamaged(float currentHP, DamageInfo damageInfo)
    {
        ChangeMovementState(new HurtState());
        Velocity += damageInfo.Knockback;
    }

    public abstract void OnDied(DamageInfo damageInfo);

    public override void _Process(double delta)
    {
        ActivateWeaponStrategy.Activate(MyWeapon, this);
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _runStateMachines(delta);
        MoveAndSlide();
        Velocity = Velocity.Lerp(Vector2.Zero, Friction);
    }

    private void _runStateMachines(double delta)
    {
        if (!ActionStateMachine.IsAllLayerState())
        {
            MovementStateMachine.Update(delta);
        }
        ActionStateMachine.Update(delta);
    }

    public void Move()
    {
        MovDirection = MovDirection.Normalized();
        Velocity = Velocity.Lerp(_maxSpeed * MovDirection, _acceleration);
        // Velocity += MovDirection * _acceleration;
        Velocity = Velocity.LimitLength(_maxSpeed);
    }

    public void GetDirection()
    {
        this.MovementStrategy.GetDirection();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        Health.TakeDamage(damageInfo);
    }

    // Allow state classes to change the current movement state while still keeping the
    // state machine encapsulated within the Character class.
    public void ChangeMovementState(IMovementState newState)
    {
        MovementStateMachine?.SetState(newState);
    }

    public void ChangeActionState(IActionState newState)
    {
        ActionStateMachine?.SetState(newState);
    }

    public IMovementState GetMovementState()
    {
        return MovementStateMachine?.CurrentState;
    }

    public IActionState GetActionState()
    {
        return ActionStateMachine?.CurrentState;
    }

    public float GetHP()
    {
        return Health.CurrentHP;
    }

    public float GetMaxHP()
    {
        return Health.MaxHP;
    }

    public abstract void PlayIdleAnimation();
    public abstract void PlayMoveAnimation();
    public abstract Task PlayDeathAnimation();
}