using Godot;
using nuscutiesapp.active.characters.ActivateWeaponStrategies;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using nuscutiesapp.active.characters.Weapons;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Player : Character
{
    private ActiveDungeonEventManager _eventManager;
    private DerivedStatCalculator _statCalculator;
    public override void _Ready()
    {
        base._Ready();
        _statCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        MyWeapon = WeaponCreator.CreateSword(this, new DamageFunction(_statCalculator.CalcAttackDamageMultiplier() * 10f));
        AddChild(MyWeapon);
        MovementStrategy = new PlayerMovementStrategy(this);

        MovementStateMachine = new StateMachine<IMovementState>(this, new IdleMovementState());
        ActionStateMachine = new StateMachine<IActionState>(this, new IdleActionState());
        this._eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");

        ActivateWeaponStrategy = new InputActivateWeaponStrategy();
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

        MyWeapon.Rotation = mouseDirection.Angle();
        if (mouseDirection.X < 0 && MyWeapon.Scale.Y > 0)
        {
            MyWeapon.ApplyScale(new Vector2(1, -1));
        }
        else if (mouseDirection.X > 0 && MyWeapon.Scale.Y < 0)
        {
            MyWeapon.ApplyScale(new Vector2(1, -1));
        }
        base._Process(delta);
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
        this.MyWeapon.Visible = false;
        MyAnimationPlayer.Play("die");
        await ToSignal(MyAnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
    }

    public override void OnDied(DamageInfo damageInfo)
    {
        ActionStateMachine.SetState(new DeadState());
        _eventManager.GameLost();
    }
}
