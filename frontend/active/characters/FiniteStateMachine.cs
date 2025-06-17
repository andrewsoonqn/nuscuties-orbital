using Godot;
using System.Collections.Generic;

public partial class FiniteStateMachine : Node
{
    protected Dictionary<string, int> _states = new Dictionary<string, int>();

    protected int _previousStateId = -1;

    protected int _internalStateId = -1;

    public int CurrentStateId
    {
        get => _internalStateId;
        set => SetState(value); // When CurrentStateId is set, it calls the SetState method.
    }

    protected Character _parentCharacter;

    protected AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _parentCharacter = GetParent<Character>();

        if (_parentCharacter != null)
        {
            _animationPlayer = _parentCharacter.GetNode<AnimationPlayer>("AnimationPlayer");
            if (_animationPlayer == null)
            {
                GD.PrintErr(
                    $"FiniteStateMachine: AnimationPlayer node not found as a child of '{_parentCharacter.Name}'.");
            }
        }
        else
        {
            GD.PrintErr("FiniteStateMachine: Parent node is not of type Character or is null.");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_internalStateId != -1)
        {
            GD.Print(_parentCharacter, _internalStateId);
            StateLogic(delta);

            int transition = GetTransition();
            if (transition != -1)
            {
                SetState(transition);
            }
        }

    }

    protected virtual void StateLogic(double delta)
    {
    }

    protected virtual int GetTransition()
    {
        return -1;
    }

    public void AddState(string newStateName)
    {
        _states[newStateName] = _states.Count;
    }

    public void SetState(int newStateId)
    {
        ExitState(_internalStateId);
        _previousStateId = _internalStateId;
        _internalStateId = newStateId;
        EnterState(_previousStateId, _internalStateId);
    }

    protected virtual void EnterState(int previousStateId, int newStateId)
    {
    }

    protected virtual void ExitState(int stateExitedId)
    {
    }
}