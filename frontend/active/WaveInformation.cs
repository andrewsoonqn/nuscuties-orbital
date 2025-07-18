using Godot;
using nuscutiesapp.active;
using System;

public partial class WaveInformation : Node
{
    private ActiveDungeonEventManager _eventManager;
    private EnemySpawner _enemySpawner;
    private Label _wavesLabel;
    private Label _enemiesSpawnedLabel;

    private int _enemiesRemaining;
    private int _waves;

    private int _totalWaves;
    private bool _eventsSubscribed = false;

    public override void _Ready()
    {
        Node parent = GetParent();
        if (parent != null)
        {
            _wavesLabel = parent.FindChild("Waves") as Label;
            _enemiesSpawnedLabel = parent.FindChild("RemainingEnemies") as Label;
        }

        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");

        _eventsSubscribed = false;

        InitializeWaveInformation();

        base._Ready();
    }

    public void InitializeWaveInformation()
    {
        UnsubscribeFromEvents();

        if (GetParent() != null && GetParent().HasNode("GameWorld"))
        {
            _totalWaves = GetParent().GetNode<ActiveGame>("GameWorld").GetMaxWaves();
        }

        if (_wavesLabel != null)
        {
            _wavesLabel.Text = $"Wave 0/{_totalWaves}";
        }

        if (_enemiesSpawnedLabel != null)
        {
            _enemiesSpawnedLabel.Text = $"Enemies Spawning...";
        }

        if (_eventManager != null)
        {
            _eventManager.EnemySpawnedEvent += OnEnemySpawned;
            _eventManager.WaveElapsedEvent += OnWaveElapsed;
            _eventManager.EnemyDiedEvent += OnEnemyDied;
            _eventsSubscribed = true;
        }
        else
        {
            GD.PrintErr("WaveInformation: ERROR - Cannot subscribe events, event manager is null");
        }
    }

    public void OnEnemyDied()
    {
        _enemiesRemaining--;

        if (_enemiesSpawnedLabel != null)
        {
            _enemiesSpawnedLabel.Text = $"{_enemiesRemaining} enemies remaining";
        }

        if (_enemiesRemaining <= 0)
        {
            _eventManager.GameWon();
        }
    }

    public void OnEnemySpawned()
    {
        _enemiesRemaining++;

        if (_enemiesSpawnedLabel != null)
        {
            _enemiesSpawnedLabel.Text = $"{_enemiesRemaining} enemies remaining";
        }
    }

    public void OnWaveElapsed()
    {
        _waves++;

        if (_wavesLabel != null)
        {
            _wavesLabel.Text = $"Wave {_waves}/{_totalWaves}";
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (_eventManager != null && _eventsSubscribed)
        {
            _eventManager.EnemySpawnedEvent -= OnEnemySpawned;
            _eventManager.WaveElapsedEvent -= OnWaveElapsed;
            _eventManager.EnemyDiedEvent -= OnEnemyDied;
            _eventsSubscribed = false;
        }
    }

    public override void _ExitTree()
    {
        UnsubscribeFromEvents();

        _wavesLabel = null;
        _enemiesSpawnedLabel = null;

        base._ExitTree();
    }
}