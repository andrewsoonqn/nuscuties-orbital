using System.Numerics;

namespace nuscutiesapp.active
{
    using Godot;
    using System;

    public partial class EnemySpawner : Node2D
    {
        [Export]
        public PackedScene EnemyScene { get; set; } 

        [Export]
        public Timer SpawnTimer { get; set; } 
        
        [Export]
        public NavigationRegion2D NavigationRegion { get; set; }
        
        [Export]
        public Marker2D TopLeftMarker { get; set; }

        public override void _Ready()
        {
            if (SpawnTimer != null)
            {
                SpawnTimer.Timeout += OnSpawnTimerTimeout;
                SpawnTimer.Start();
            }
        }

        private void OnSpawnTimerTimeout()
        {
            SpawnEnemy();
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