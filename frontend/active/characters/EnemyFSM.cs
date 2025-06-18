using Godot;
using System.Diagnostics;

public partial class EnemyFSM : FiniteStateMachine
{
    public EnemyFSM()
    {
        AddState("idle");
        AddState("move");
    }

    public override void _Ready()
    {
        base._Ready();
        _animationPlayer.Play("fly");
        SetState(_states["idle"]);
    }

    protected override void StateLogic(double delta)
    {
        if (CurrentStateId == _states["idle"] || CurrentStateId == _states["move"])
        {
            _parentCharacter.GetInput();
            _parentCharacter.Move();
            GD.Print("velocity", _parentCharacter.Velocity);
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
        }
        else if (newStateId == _states["move"])
        {
        }
    }
}