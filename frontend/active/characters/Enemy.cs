using Godot;
using nuscutiesapp.active.characters.MovementStrategies;
using nuscutiesapp.active.characters.StateLogic;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Enemy : Character
{
    private NavigationAgent2D _navigationAgent;
    private Node2D _target;

    public override void _Ready()
    {
        this.CallDeferred(nameof(SeekerSetup));
        this._navigationAgent = this.GetNode<NavigationAgent2D>("NavigationAgent2D");
        this._target = this.GetParent().GetNode<Node2D>("Player");
        MovementStrategy = new SeekTargetMovementStrategy(this, _target, _navigationAgent);
        base._Ready();
        MovementStateMachine = new StateMachine<IMovementState>(this, new IdleState());
        ActionStateMachine = new StateMachine<IActionState>(this, new IdleState());
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
}