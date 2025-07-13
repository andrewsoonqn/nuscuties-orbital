using Godot;
using nuscutiesapp.active;
using System;

public partial class WaveInformation : Node
{
    private ActiveDungeonEventManager _eventManager;
    [Export] private Label _wavesLabel;
    [Export] private Label _enemiesSpawnedLabel;
    
    private int _enemiesRemaining;
    private int _waves;

    public override void _Ready()
    {
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _wavesLabel.Text = $"Wave 0";
        _enemiesSpawnedLabel.Text = $"Enemies Spawning...";
        
        _eventManager.EnemySpawnedEvent += OnEnemySpawned;
        _eventManager.WaveElapsedEvent += OnWaveElapsed;
        _eventManager.EnemyDiedEvent += OnEnemyDied;
        
        base._Ready();
    }

    public void OnEnemyDied()
    {
        _enemiesRemaining--;
        _enemiesSpawnedLabel.Text = $"{_enemiesRemaining} enemies remaining";

        if (_enemiesRemaining <= 0)
        {
            _eventManager.GameWon();
        }
    }
    public void OnEnemySpawned()
    {
        _enemiesRemaining++;
        _enemiesSpawnedLabel.Text = $"{_enemiesRemaining} enemies remaining";
    }

    public void OnWaveElapsed()
    {
        _waves++;
        _wavesLabel.Text = $"Wave {_waves}";
    }
}
