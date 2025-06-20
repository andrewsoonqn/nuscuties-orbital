using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using System;

public partial class Hitbox : Area2D
{
    public bool monitoring = false;
    [Export] private float damage;
    public Character Wielder;
    private Vector2 knockbackDirection;
    [Export] private float knockbackMagnitude;
    
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        this._collisionShape = this.GetNode<CollisionShape2D>("CollisionShape2D");
        this.BodyEntered += OnBodyEntered;
        GD.Print("hi im here, my wielder is " + Wielder + " and my parent is " + GetParent().Name);
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print("hitbox entered");
        if (body is Character character && monitoring)
        {
            GD.Print("char v ", Wielder.Velocity);
            this.knockbackDirection = character.GlobalPosition - Wielder.GlobalPosition; // TODO change this
            knockbackDirection = knockbackDirection.Normalized();
            GD.Print("knockback ", knockbackDirection);
            DamageInfo damage = new DamageInfo(
                this.damage, knockbackDirection * knockbackMagnitude
            );
            character.TakeDamage(damage);
        }
    }
}
