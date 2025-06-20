using Godot;
using System;

public partial class Sword : Node2D
{
    private Hitbox _hitbox;
    private AnimationPlayer _animationPlayer;
    public override void _Ready()
    {
        _hitbox = GetNode<Hitbox>("Hitbox");
        _hitbox.Wielder = this.GetParent<Character>();
        _hitbox.monitoring = false;
        
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.AnimationStarted += OnAnimationStarted;
        _animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animName)
    {
        _hitbox.monitoring = false;
    }

    private void OnAnimationStarted(StringName animName)
    {
        _hitbox.monitoring = true;
    }
}
