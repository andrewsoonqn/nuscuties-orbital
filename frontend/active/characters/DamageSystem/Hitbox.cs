using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;
using System;

public partial class Hitbox : Area2D
{
    public bool monitoring = false;
    private Character _wielder;
    private Vector2 _knockbackDirection;
    private float _knockbackMagnitude;
    private Func<float> _damageFunc;

    protected DerivedStatCalculator _derivedStatCalculator;

    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        this._collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.BodyEntered += OnBodyEntered;

        _derivedStatCalculator = this.GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
    }

    public void Initialize(Character wielder, Func<float> damageFunc, float knockbackMagnitude)
    {
        this._wielder = wielder;
        this._damageFunc = damageFunc;
        this._knockbackMagnitude = knockbackMagnitude;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Character character && monitoring)
        {
            this._knockbackDirection = character.GlobalPosition - _wielder.GlobalPosition; // TODO change this
            _knockbackDirection = _knockbackDirection.Normalized();

            float damageAmt = _damageFunc();
            DamageInfo damageInfo = new DamageInfo(
                damageAmt, _knockbackDirection * _knockbackMagnitude
            );
            character.TakeDamage(damageInfo);
        }
    }
}