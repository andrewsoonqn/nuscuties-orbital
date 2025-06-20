using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class Sword : Node2D
{
    [Export] private Hitbox _hitbox;
    private AnimationPlayer _animationPlayer;
    private const int AttackDurationMs = 250;
    public override void _Ready()
    {
        this.Visible = true;

        _hitbox.Wielder = this.GetParent<Character>();
        _hitbox.monitoring = false;

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animationPlayer.AnimationStarted += OnAnimationStarted;
    }

    private async void OnAttackFinished(StringName animName)
    {
        await Task.Delay(AttackDurationMs);
        _hitbox.monitoring = false;
    }

    private void OnAnimationStarted(StringName animName)
    {
        CallDeferred(MethodName.OnAttackFinished, animName);
        _hitbox.monitoring = true;
    }
}