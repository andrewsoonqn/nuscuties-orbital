using Godot;
using nuscutiesapp.active;
using nuscutiesapp.tools;
using System;
using System.Collections.Generic;

namespace nuscutiesapp.active.drops
{
    public partial class DropManager : Node
    {
        private PlayerInventoryManager _inventoryManager;
        private Random _random = new Random();

        private Dictionary<Type, PackedScene> _dropScenes = new();

        private const float COIN_DROP_CHANCE = 1.0f;
        private const float HEALTH_POTION_CHANCE = 0.15f;
        private const float BUFF_POTION_CHANCE = 0.08f;
        private const float FORCEFIELD_POTION_CHANCE = 0.05f;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
            LoadDropScenes();
        }

        private void LoadDropScenes()
        {
            _dropScenes[typeof(CoinDrop)] = GD.Load<PackedScene>("res://active/drops/coin_drop.tscn");
            _dropScenes[typeof(HealthPotion)] = GD.Load<PackedScene>("res://active/drops/health_potion.tscn");
            _dropScenes[typeof(BuffPotion)] = GD.Load<PackedScene>("res://active/drops/buff_potion.tscn");
            _dropScenes[typeof(ForcefieldPotion)] = GD.Load<PackedScene>("res://active/drops/forcefield_potion.tscn");
            _dropScenes[typeof(VictoryOrb)] = GD.Load<PackedScene>("res://active/drops/victory_orb.tscn");
        }

        public void SpawnDropsFromEnemyDeath(Vector2 position, Node2D world, bool isLastEnemy = false)
        {
            if (isLastEnemy)
            {
                SpawnVictoryOrb(position, world);
                return;
            }

            SpawnCoinDrop(position, world);

            if (_random.NextDouble() < HEALTH_POTION_CHANCE)
            {
                SpawnDrop<HealthPotion>(position, world);
            }

            if (_random.NextDouble() < BUFF_POTION_CHANCE)
            {
                SpawnDrop<BuffPotion>(position, world);
            }

            if (_random.NextDouble() < FORCEFIELD_POTION_CHANCE)
            {
                SpawnDrop<ForcefieldPotion>(position, world);
            }
        }

        private void SpawnCoinDrop(Vector2 position, Node2D world)
        {
            if (_dropScenes.TryGetValue(typeof(CoinDrop), out PackedScene coinScene))
            {
                var coinDrop = coinScene.Instantiate<CoinDrop>();
                world.AddChild(coinDrop);
                coinDrop.GlobalPosition = position + GetRandomOffset();

                int coinValue = _inventoryManager.CalculateActiveDungeonEnemyCoinReward();
                coinDrop.Initialize(coinValue);
            }
        }

        private void SpawnDrop<T>(Vector2 position, Node2D world) where T : BaseDropItem
        {
            if (_dropScenes.TryGetValue(typeof(T), out PackedScene dropScene))
            {
                var drop = dropScene.Instantiate<T>();
                world.AddChild(drop);
                drop.GlobalPosition = position + GetRandomOffset();
            }
        }

        private void SpawnVictoryOrb(Vector2 position, Node2D world)
        {
            if (_dropScenes.TryGetValue(typeof(VictoryOrb), out PackedScene victoryScene))
            {
                var victoryOrb = victoryScene.Instantiate<VictoryOrb>();
                world.AddChild(victoryOrb);
                victoryOrb.GlobalPosition = position;
                GD.Print("Victory Orb spawned for the last enemy!");
            }
        }

        private Vector2 GetRandomOffset()
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float distance = (float)(_random.NextDouble() * 20 + 10);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        }
    }
}