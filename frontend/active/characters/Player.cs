using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using System;
using System.Threading.Tasks;

public partial class Player : Character
{
    private Node2D _sword;
    private AnimationPlayer _swordAnimationPlayer;
    private ActiveDungeonEventManager _eventManager;
    public override void _Ready()
    {
        base._Ready();
        _sword = this.GetNode<Node2D>("Sword");
        _swordAnimationPlayer = _sword.GetNode<AnimationPlayer>("AnimationPlayer");
        _sword.GetNode<Sprite2D>("SlashSprite").Visible = false;
        MovementStrategy = new PlayerMovementStrategy(this);

        MovementStateMachine = new StateMachine<IMovementState>(this, new IdleMovementState());
        ActionStateMachine = new StateMachine<IActionState>(this, new IdleActionState());
        this._eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
    }

    public override void _Process(double delta)
    {
        Vector2 mouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();

        if (mouseDirection.X > 0 && AnimatedSprite.FlipH)
        // if (MovDirection.X > 0 && AnimatedSprite.FlipH)
        {
            AnimatedSprite.FlipH = false;

        }
        // else if (MovDirection.X < 0 && !AnimatedSprite.FlipH)
        else if (mouseDirection.X < 0 && !AnimatedSprite.FlipH)
        {
            AnimatedSprite.FlipH = true;
        }

        _sword.Rotation = mouseDirection.Angle();
        if (mouseDirection.X < 0 && _sword.Scale.Y > 0)
        {
            _sword.ApplyScale(new Vector2(1, -1));
        }
        else if (mouseDirection.X > 0 && _sword.Scale.Y < 0)
        {
            _sword.ApplyScale(new Vector2(1, -1));
        }

        if (Input.IsActionJustPressed("ui_attack") && !_swordAnimationPlayer.IsPlaying())
        {
            _swordAnimationPlayer.Play("attack");
        }
    }

    public override void PlayIdleAnimation()
    {
        MyAnimationPlayer.Play("idle");
    }
    public override void PlayMoveAnimation()
    {
        MyAnimationPlayer.Play("walk");
    }
    public override async Task PlayDeathAnimation()
    {
        this._sword.Visible = false;
        MyAnimationPlayer.Play("die");
        await ToSignal(MyAnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
    }

    public override void OnDied(DamageInfo damageInfo)
    {
        ActionStateMachine.SetState(new DeadState());
        _eventManager.GameLost();
    }
}