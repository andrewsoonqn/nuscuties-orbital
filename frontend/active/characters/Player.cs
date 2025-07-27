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
using System.Runtime.Versioning;
using System.Threading.Tasks;

public partial class Player : Character
{
    private ActiveDungeonEventManager _eventManager;
    private DerivedStatCalculator _statCalculator;
    private LoadoutSpawner _loadoutSpawner;
    private LoadoutSpawner.LoadoutData _currentLoadout;

    private Dictionary<WeaponClass, Weapon> _weapons = new Dictionary<WeaponClass, Weapon>();
    public override void _Ready()
    {
        AddToGroup("player");
        base._Ready();
        _statCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        _loadoutSpawner = GetNode<LoadoutSpawner>("/root/LoadoutSpawner");

        _currentLoadout = _loadoutSpawner.SpawnLoadout(this);

        if (_currentLoadout.MeleeWeapon != null)
        {
            _weapons[WeaponClass.Melee] = _currentLoadout.MeleeWeapon;
        }

        if (_currentLoadout.ProjectileWeapon != null)
        {
            _weapons[WeaponClass.Ranged] = _currentLoadout.ProjectileWeapon;
        }

        MyWeapon = _currentLoadout.MeleeWeapon ?? _currentLoadout.ProjectileWeapon;
        if (MyWeapon != null)
        {
            AddChild(MyWeapon);
        }

        _loadoutSpawner.ApplyLoadout(this, _currentLoadout);

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

        if (MyWeapon != null)
        {
            MyWeapon.Rotation = mouseDirection.Angle();
            if (mouseDirection.X < 0 && MyWeapon.Scale.Y > 0)
            {
                MyWeapon.ApplyScale(new Vector2(1, -1));
            }
            else if (mouseDirection.X > 0 && MyWeapon.Scale.Y < 0)
            {
                MyWeapon.ApplyScale(new Vector2(1, -1));
            }
        }

        HandleActiveAbilityInput();
        base._Process(delta);
    }

    private void HandleActiveAbilityInput()
    {
        if (Input.IsActionJustPressed("active_ability") && _currentLoadout?.ActiveAbilities != null)
        {
            foreach (var ability in _currentLoadout.ActiveAbilities)
            {
                if (ability != null)
                {
                    ability.Activate();
                    break;
                }
            }
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
        this.MyWeapon.Visible = false;
        MyAnimationPlayer.Play("die");
        await ToSignal(MyAnimationPlayer, AnimationPlayer.SignalName.AnimationFinished);
    }

    public override void OnDied(DamageInfo damageInfo)
    {
        ActionStateMachine.SetState(new DeadState());
        _eventManager.GameLost();
    }

    public void EquipWeapon(Weapon weapon)
    {
        RemoveChild(MyWeapon);
        MyWeapon = weapon;
        AddChild(MyWeapon);
    }
    public void SwitchWeapon(WeaponClass weaponClass)
    {
        if (_weapons[weaponClass] != null)
        {
            EquipWeapon(_weapons[weaponClass]);
        }
    }

    public void ReloadLoadout()
    {
        if (_currentLoadout != null)
        {
            _loadoutSpawner.RemoveLoadout(this, _currentLoadout);
        }

        _currentLoadout = _loadoutSpawner.SpawnLoadout(this);

        _weapons.Clear();

        if (_currentLoadout.MeleeWeapon != null)
        {
            _weapons[WeaponClass.Melee] = _currentLoadout.MeleeWeapon;
        }

        if (_currentLoadout.ProjectileWeapon != null)
        {
            _weapons[WeaponClass.Ranged] = _currentLoadout.ProjectileWeapon;
        }

        if (MyWeapon != null)
        {
            RemoveChild(MyWeapon);
        }

        MyWeapon = _currentLoadout.MeleeWeapon ?? _currentLoadout.ProjectileWeapon;
        if (MyWeapon != null)
        {
            AddChild(MyWeapon);
        }

        _loadoutSpawner.ApplyLoadout(this, _currentLoadout);
    }

    public LoadoutSpawner.LoadoutData GetCurrentLoadout()
    {
        return _currentLoadout;
    }

    public override void _ExitTree()
    {
        if (_currentLoadout != null && _loadoutSpawner != null)
        {
            _loadoutSpawner.RemoveLoadout(this, _currentLoadout);
        }
        base._ExitTree();
    }
}
