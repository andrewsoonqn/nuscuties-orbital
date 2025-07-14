using Godot;
using nuscutiesapp.active;
using System;

public partial class WaveInformation : Node
{
    private ActiveDungeonEventManager _eventManager;
    private EnemySpawner _enemySpawner;
    [Export] private Label _wavesLabel;
    [Export] private Label _enemiesSpawnedLabel;
    
    private int _enemiesRemaining;
    private int _waves;

    private int _totalWaves;

    public override void _Ready()
    {
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");

        _totalWaves = GetParent().GetNode<ActiveGame>("GameWorld").GetMaxWaves();
        _wavesLabel.Text = $"Wave 0/{_totalWaves}";
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
        _wavesLabel.Text = $"Wave {_waves}/{_totalWaves}";
    }
}
