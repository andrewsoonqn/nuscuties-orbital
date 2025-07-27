using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.StatusEffects;
using nuscutiesapp.tools;
using System;

public partial class Hitbox : Area2D
{
    public bool monitoring = true;
    [Export] private Character _wielder;
    [Export] private Vector2 _knockbackDirection;
    [Export] private float _knockbackMagnitude;
    [Export] private DamageFunction _damageFunc;

    [Export] private string _statusEffect;

    protected DerivedStatCalculator _derivedStatCalculator;

    [Export] private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        this._collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.BodyEntered += OnBodyEntered;

        _derivedStatCalculator = this.GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
    }

    public void Initialize(Character wielder, DamageFunction damageFunc, float knockbackMagnitude, string statusEffect = null)
    {
        this._wielder = wielder;
        this._damageFunc = damageFunc;
        this._knockbackMagnitude = knockbackMagnitude;
        this._statusEffect = statusEffect;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Character character && monitoring)
        {
            this._knockbackDirection = character.GlobalPosition - _wielder.GlobalPosition;
            _knockbackDirection = _knockbackDirection.Normalized();

            float damageAmt = _damageFunc.CalculateDamage();

            if (_wielder.StatusEffects != null)
            {
                var buffEffect = _wielder.StatusEffects.GetStatusEffect<BuffStatusEffect>();
                if (buffEffect != null && buffEffect.IsActive)
                {
                    damageAmt *= buffEffect.GetDamageMultiplier();
                }
            }

            DamageInfo damageInfo = new DamageInfo(
                damageAmt, _knockbackDirection * _knockbackMagnitude, _statusEffect
            );
            character.TakeDamage(damageInfo);

            if (!string.IsNullOrEmpty(_statusEffect) && character.StatusEffects != null)
            {
                ApplyStatusEffect(character, _statusEffect);
            }
        }
    }

    private void ApplyStatusEffect(Character target, string effectType)
    {
        switch (effectType.ToLower())
        {
            case "burn":
                target.StatusEffects.ApplyStatusEffect<BurnStatusEffect>();
                break;
            case "slow":
                target.StatusEffects.ApplyStatusEffect<SlowStatusEffect>();
                break;
            default:
                GD.PrintErr($"Unknown status effect: {effectType}");
                break;
        }
    }
}