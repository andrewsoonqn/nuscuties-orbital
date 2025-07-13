using Godot;
using nuscutiesapp.active;
using System;

public partial class WaveInformation : Node
{
    private ActiveDungeonEventManager _eventManager;
    [Export] private Label _wavesLabel;
    [Export] private Label _enemiesSpawnedLabel;
    
    private int _enemiesSpawned;
    private int _waves;

    public override void _Ready()
    {
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _wavesLabel.Text = $"Wave 0";
        _enemiesSpawnedLabel.Text = $"Enemies Spawning...";
        
        _eventManager.EnemySpawnedEvent += OnEnemySpawned;
        _eventManager.WaveElapsedEvent += OnWaveElapsed;
        
        base._Ready();
    }

    public void OnEnemySpawned()
    {
        _enemiesSpawned++;
        _enemiesSpawnedLabel.Text = $"{_enemiesSpawned} enemies remaining";
    }

    public void OnWaveElapsed()
    {
        _waves++;
        _wavesLabel.Text = $"Wave {_waves}";
    }
}
