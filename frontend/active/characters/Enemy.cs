using Godot;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class Enemy : Character
{
    private NavigationAgent2D _navigationAgent;
    private Node2D _target;
    [Export] private int Speed = 10;
    public override void _Ready()
    {
        this.CallDeferred(nameof(SeekerSetup));
        this._navigationAgent = this.GetNode<NavigationAgent2D>("NavigationAgent2D");
        this._target = this.GetParent().GetNode<Node2D>("Player");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target != null)
        {
            _navigationAgent.TargetPosition = _target.GlobalPosition;
        }

        if (_navigationAgent.IsNavigationFinished()) return;

        Vector2 currentAgentPosition = GlobalPosition;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * Speed;
        MoveAndSlide();
    }

    public async void SeekerSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        if (_target != null)
        {
            _navigationAgent.TargetPosition = _target.GlobalPosition;
        }
    }
}