using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;
using System;

public partial class Hitbox : Area2D
{
    public bool monitoring = false;
    [Export] private float damage;
    public Character Wielder;
    private Vector2 knockbackDirection;
    [Export] private float knockbackMagnitude;

    private DerivedStatCalculator _derivedStatCalculator;

    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        this._collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.BodyEntered += OnBodyEntered;
        
        _derivedStatCalculator = this.GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Character character && monitoring)
        {
            this.knockbackDirection = character.GlobalPosition - Wielder.GlobalPosition; // TODO change this
            knockbackDirection = knockbackDirection.Normalized();
            
            // TODO use a pattern here
            float damageAmt;
            if (Wielder is Player)
            {
                damageAmt = this.damage * _derivedStatCalculator.CalcAttackDamageMultiplier();
            }
            else
            {
                damageAmt = this.damage;
            }
            
            DamageInfo damageInfo = new DamageInfo(
                damageAmt, knockbackDirection * knockbackMagnitude
            );
            character.TakeDamage(damageInfo);
        }
    }
}