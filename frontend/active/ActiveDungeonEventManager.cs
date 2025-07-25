using Godot;
using System;
using System.Collections.Generic;

public partial class ActiveDungeonEventManager : Node
{
    public void GameLost()
    {
        EmitSignal(SignalName.GameLostEvent);
    }

    [Signal]
    public delegate void GameLostEventEventHandler();

    public void EnemyDied()
    {
        EmitSignal(SignalName.EnemyDiedEvent);
    }

    [Signal]
    public delegate void EnemyDiedEventEventHandler();

    public void GameWon()
    {
        EmitSignal(SignalName.GameWonEvent);
    }

    [Signal]
    public delegate void GameWonEventEventHandler();


    public void GameStarted()
    {
        EmitSignal(SignalName.GameStartedEvent);
    }

    [Signal]
    public delegate void EnemySpawnedEventEventHandler();

    public void EnemySpawned()
    {
        EmitSignalEnemySpawnedEvent();
    }

    [Signal]
    public delegate void WaveElapsedEventEventHandler();

    public void WaveElapsed()
    {
        EmitSignalWaveElapsedEvent();
    }

    [Signal]
    public delegate void GameStartedEventEventHandler();

    public void WaveStart(int wave)
    {
        EmitSignalWaveStartEvent(wave);
    }

    [Signal]
    public delegate void WaveStartEventEventHandler(int wave);
}