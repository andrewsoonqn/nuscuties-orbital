using System.Numerics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
        public TileMapLayer FloorTileMap { get; set; }

        [Export]
        public TileMapLayer WallTileMap { get; set; }

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

        private ActiveDungeonEventManager _eventManager;
        private List<Vector2I> _navigatableCells;

        public override void _Ready()
        {
            _spawnTimer = new Timer();
            _spawnTimer.WaitTime = WaveLength / EnemiesPerWave;
            AddChild(_spawnTimer);

            _spawnTimer.Timeout += OnSpawnTimerTimeout;

            _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");

            CalculateNavigatableCells();

            WaveCycle();
        }

        private void CalculateNavigatableCells()
        {
            var floorCells = FloorTileMap.GetUsedCells();
            var wallCells = WallTileMap.GetUsedCells();

            var wallCellsSet = new HashSet<Vector2I>(wallCells);

            _navigatableCells = floorCells.Where(cell => !wallCellsSet.Contains(cell)).ToList();
        }

        private Vector2I? GetRandomNavigatableCell()
        {
            if (_navigatableCells == null || _navigatableCells.Count == 0)
            {
                GD.PrintErr("No navigatable cells available for spawning");
                return null;
            }

            var random = new Random();
            int randomIndex = random.Next(0, _navigatableCells.Count);
            return _navigatableCells[randomIndex];
        }

        private async Task WaveCycle()
        {
            if (_totalWavesDone >= Waves)
            {
                _spawnTimer.Stop();
                // _eventManager.GameWon();
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

            Vector2I? randomCell = GetRandomNavigatableCell();
            if (randomCell == null)
            {
                GD.PrintErr("Could not find a navigatable cell for enemy spawning");
                enemy.QueueFree();
                return;
            }

            Vector2 spawnPosition = FloorTileMap.MapToLocal(randomCell.Value);
            GD.Print("val ", randomCell.Value);

            GetParent().AddChild(enemy);
            
            enemy.Position = spawnPosition;

            GD.Print("enemy pos ", enemy.Position, " global ", enemy.GlobalPosition);
        }
    }
}
