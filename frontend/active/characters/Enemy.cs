using Godot;
using nuscutiesapp.active;
using nuscutiesapp.active.characters.ActivateWeaponStrategies;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using nuscutiesapp.active.characters.Weapons;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.active.drops;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Enemy : Character
{
    private NavigationAgent2D _navigationAgent;
    private Node2D _target;
    private ActiveDungeonEventManager _eventManager;
    [Export] private Area2D _area2D;

    private EnemyHealthBar _healthBar;
    private static readonly PackedScene _healthBarScene = GD.Load<PackedScene>("res://active/characters/enemy_health_bar.tscn");

    public override void _Ready()
    {
        this.CallDeferred(nameof(SeekerSetup));
        this._navigationAgent = this.GetNode<NavigationAgent2D>("NavigationAgent2D");
        this._target = this.GetParent().GetNode<Node2D>("Player");
        MovementStrategy = new SeekTargetMovementStrategy(this, _target, _navigationAgent);
        base._Ready();
        MovementStateMachine = new StateMachine<IMovementState>(this, new IdleMovementState());
        ActionStateMachine = new StateMachine<IActionState>(this, new IdleActionState());
        this._eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");

        MyWeapon = Weapon.CreateWeapon(
            Weapon.WeaponType.Fist,
            this,
            new DamageFunction(() => 10),
            200,
            100,
            new NoAnimationUseStrategy()
            );
        AddChild(MyWeapon);

        ActivateWeaponStrategy = new AlwaysActivateWeaponStrategy();

        SetupHealthBar();
    }

    private void SetupHealthBar()
    {
        _healthBar = _healthBarScene.Instantiate<EnemyHealthBar>();
        AddChild(_healthBar);
        _healthBar.Initialize(this);
    }

    public async void SeekerSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        if (_target != null)
        {
            _navigationAgent.TargetPosition = _target.GlobalPosition;
        }
    }
    public override void PlayIdleAnimation()
    {
        MyAnimationPlayer.Play("fly");
    }
    public override void PlayMoveAnimation()
    {
        MyAnimationPlayer.Play("fly");
    }

    public override async Task PlayDeathAnimation()
    {
        MyAnimationPlayer.Play("die");
        await ToSignal(MyAnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
    }

    public override void OnDied(DamageInfo damageInfo)
    {
        ActionStateMachine.SetState(new DeadState());

        var enemyTracker = GetNode<EnemyTracker>("/root/EnemyTracker");
        var dropManager = GetNode<DropManager>("/root/DropManager");

        if (dropManager != null && enemyTracker != null)
        {
            int remainingEnemies = enemyTracker.GetEnemyCount() - 1;
            bool isLastEnemy = remainingEnemies <= 0;

            dropManager.SpawnDropsFromEnemyDeath(GlobalPosition, GetParent<Node2D>(), isLastEnemy);
        }

        _eventManager.EnemyDied();
    }

}