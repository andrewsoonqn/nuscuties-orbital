using Godot;
using System.Diagnostics;

public partial class PlayerFSM : FiniteStateMachine
{
    public PlayerFSM()
    {
        AddState("idle");
        AddState("move");
    }

    public override void _Ready()
    {
        base._Ready();
        SetState(_states["idle"]);
        GD.Print($"PlayerFSM state: {_internalStateId}");
    }

    protected override void StateLogic(double delta)
    {
        if (CurrentStateId == _states["idle"] || CurrentStateId == _states["move"])
        {
            _parentCharacter.GetInput();
            _parentCharacter.Move();
        }
    }

    protected override int GetTransition()
    {
        if (CurrentStateId == _states["idle"])
        {
            if (_parentCharacter.Velocity.Length() > 10)
            {
                return _states["move"];
            }
        }
        else if (CurrentStateId == _states["move"])
        {
            if (_parentCharacter.Velocity.Length() < 10)
            {
                return _states["idle"];
            }
        }
        return -1; // Won't reach here
    }

    protected override void EnterState(int previousStateId, int newStateId)
    {
        if (newStateId == _states["idle"])
        {
            _animationPlayer.Play("idle");
        } else if (newStateId == _states["move"])
        {
            _animationPlayer.Play("move");
        }
    }
}