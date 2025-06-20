using Godot;
using System;

public partial class RewardManager : Node
{
    private ActiveDungeonEventManager _eventManager;
    public int ExpGained { get; private set; } = 0;
    public bool GameWon {get; private set;}

    public override void _Ready()
    {
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _eventManager.EnemyDiedEvent += ActiveDungeonEventManagerOnEnemyDiedEvent;
        _eventManager.GameWonEvent += () => GameWon = true;
        _eventManager.GameLostEvent += () => GameWon = false;
        _eventManager.GameStartedEvent += () =>
        {
            GameWon = false;
            ExpGained = 0;
        };
    }

    private void ActiveDungeonEventManagerOnEnemyDiedEvent()
    {
        this.ExpGained += 1000;
    }
}
