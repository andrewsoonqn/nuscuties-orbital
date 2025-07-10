using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;
using System;

public partial class Hitbox : Area2D
{
    public bool monitoring = true;
    [Export] private Character _wielder;
    [Export] private Vector2 _knockbackDirection;
    [Export] private float _knockbackMagnitude;
    [Export] private DamageFunction _damageFunc;

    protected DerivedStatCalculator _derivedStatCalculator;

    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        this._collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.BodyEntered += OnBodyEntered;

        _derivedStatCalculator = this.GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
    }

    public void Initialize(Character wielder, DamageFunction damageFunc, float knockbackMagnitude)
    {
        this._wielder = wielder;
        this._damageFunc = damageFunc;
        this._knockbackMagnitude = knockbackMagnitude;
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print(monitoring);
        if (body is Character character && monitoring)
        {
            this._knockbackDirection = character.GlobalPosition - _wielder.GlobalPosition; // TODO change this
            _knockbackDirection = _knockbackDirection.Normalized();

            float damageAmt = _damageFunc.CalculateDamage();
            DamageInfo damageInfo = new DamageInfo(
                damageAmt, _knockbackDirection * _knockbackMagnitude
            );
            character.TakeDamage(damageInfo);
        }
    }
}