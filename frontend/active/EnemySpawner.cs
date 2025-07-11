using System.Numerics;
using System.Threading.Tasks;

namespace nuscutiesapp.active
{
    using Godot;
    using System;

    public partial class EnemySpawner : Node2D
    {
        [Export]
        public PackedScene EnemyScene { get; set; } 
        
        [Export]
        public NavigationRegion2D NavigationRegion { get; set; }
        
        [Export]
        public Marker2D TopLeftMarker { get; set; }
        
        [Export]
        public float WaveLength { get; set; }
        
        [Export]
        public int EnemiesPerWave { get; set; }
        
        [Export]
        public float TimeBetweenWaves { get; set; }
        
        [Export]
        public int Waves { get; set; }

        private Timer _spawnTimer;
        private int _enemiesSpawnedDuringWave = 0;
        private int _totalEnemiesSpawned = 0;
        private int _totalWavesDone = 0;

        public override void _Ready()
        {
            _spawnTimer = new Timer();
            _spawnTimer.WaitTime = WaveLength / EnemiesPerWave;
            AddChild(_spawnTimer);
            
            _spawnTimer.Timeout += OnSpawnTimerTimeout;
            WaveCycle();
        }

        private async Task WaveCycle()
        {
            if (_totalWavesDone >= Waves)
            {
                _spawnTimer.Stop();
                return;
            }

            _totalWavesDone++;
            _spawnTimer.Stop();
            await Task.Delay((int)(TimeBetweenWaves * 1000));
            _enemiesSpawnedDuringWave = 0;
            _spawnTimer.Start();
        }

        private async void OnSpawnTimerTimeout()
        {
            SpawnEnemy();
            GD.Print("total ", _totalEnemiesSpawned, " wave ", _enemiesSpawnedDuringWave, "waves dun", _totalWavesDone);
            _enemiesSpawnedDuringWave++;
            _totalEnemiesSpawned++;

            if (_enemiesSpawnedDuringWave == EnemiesPerWave)
            {
                await WaveCycle();
            }
        }

        private void SpawnEnemy()
        {
            if (EnemyScene == null)
            {
                GD.PrintErr("EnemyScene is not assigned in EnemySpawner.");
                return;
            }

            Node2D enemy = EnemyScene.Instantiate<Node2D>();

            Rid rid = NavigationRegion.GetRid();
            Vector2 randPoint = NavigationServer2D.RegionGetRandomPoint(rid, 0, false);
            Vector2 spawnPosition = randPoint;

            GetParent().AddChild(enemy); 
            enemy.GlobalPosition = spawnPosition;
        }
    }}