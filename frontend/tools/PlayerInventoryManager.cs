using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace nuscutiesapp.tools
{
    public partial class PlayerInventoryManager : BaseStatManager<PlayerInventoryData>
    {
        private ProgressionManager _progressionManager;

        public override void _Ready()
        {
            GD.Print(OS.GetUserDataDir());
            base._Ready();
            _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        }

        [Signal]
        public delegate void CoinsChangedEventHandler(int newTotal);

        [Signal]
        public delegate void InventoryChangedEventHandler(string category, string[] unlockedIds);

        [Signal]
        public delegate void LoadoutChangedEventHandler(string category, string equippedId);

        protected override string SaveFilePath => Path.Combine(OS.GetUserDataDir(), "player_inventory.json");

        public int Coins => Data.Coins;

        protected override void OnDataChanged()
        {
        }

        public int CalculateDailyQuestCoinReward()
        {
            int level = _progressionManager.GetLevel();
            int baseCoinReward = 50;
            double scaleFactor = 1.15;

            int reward = (int)(baseCoinReward * Math.Pow(scaleFactor, level - 1));
            return Math.Max(reward, baseCoinReward);
        }

        public int CalculatePassiveDungeonCoinReward(double timeSpentMinutes)
        {
            int level = _progressionManager.GetLevel();
            double baseCoinRate = 5.0;
            double levelMultiplier = 1.0 + (level * 0.1);
            double exponentialMultiplier = Math.Pow(timeSpentMinutes, 1.2);

            Random random = new Random();
            double variance = 0.85 + (random.NextDouble() * 0.3);

            int coins = (int)(baseCoinRate * levelMultiplier * exponentialMultiplier * variance);
            return Math.Max(coins, 1);
        }

        public int CalculateActiveDungeonEnemyCoinReward()
        {
            int level = _progressionManager.GetLevel();
            int baseCoinReward = 15;
            double scaleFactor = 1.12;

            Random random = new Random();
            double variance = 0.8 + (random.NextDouble() * 0.4);

            int reward = (int)(baseCoinReward * Math.Pow(scaleFactor, level - 1) * variance);
            return Math.Max(reward, baseCoinReward);
        }

        public bool AddCoins(int amount)
        {
            if (amount < 0)
            {
                GD.PrintErr($"Cannot add negative coins: {amount}");
                return false;
            }

            Data.Coins += amount;
            EmitSignal(SignalName.CoinsChanged, Data.Coins);
            NotifyDataChanged();
            return true;
        }

        public bool SpendCoins(int amount)
        {
            if (amount < 0)
            {
                GD.PrintErr($"Cannot spend negative coins: {amount}");
                return false;
            }

            if (Data.Coins < amount)
            {
                GD.PrintErr($"Insufficient coins. Have: {Data.Coins}, Need: {amount}");
                return false;
            }

            Data.Coins -= amount;
            EmitSignal(SignalName.CoinsChanged, Data.Coins);
            NotifyDataChanged();
            return true;
        }

        public bool UnlockItem(string itemId)
        {
            var item = ItemCatalog.Instance?.Get(itemId);
            if (item == null)
            {
                GD.PrintErr($"Item not found in catalog: {itemId}");
                return false;
            }

            if (!Data.UnlockedItems.ContainsKey(item.Category))
            {
                GD.PrintErr($"Invalid category: {item.Category}");
                return false;
            }

            if (IsUnlocked(itemId))
            {
                GD.PrintErr($"Item already unlocked: {itemId}");
                return false;
            }

            if (!SpendCoins(item.Price))
            {
                return false;
            }

            Data.UnlockedItems[item.Category].Add(itemId);
            EmitSignal(SignalName.InventoryChanged, item.Category, Data.UnlockedItems[item.Category].ToArray());
            NotifyDataChanged();
            return true;
        }

        public bool EquipItem(string itemId)
        {
            var item = ItemCatalog.Instance?.Get(itemId);
            if (item == null)
            {
                GD.PrintErr($"Item not found in catalog: {itemId}");
                return false;
            }

            if (!IsUnlocked(itemId))
            {
                GD.PrintErr($"Item not unlocked: {itemId}");
                return false;
            }

            Data.EquippedItems[item.Category] = itemId;
            EmitSignal(SignalName.LoadoutChanged, item.Category, itemId);
            NotifyDataChanged();
            return true;
        }

        public bool UnequipItem(string category)
        {
            if (!Data.EquippedItems.ContainsKey(category))
            {
                GD.PrintErr($"Invalid category: {category}");
                return false;
            }

            if (category == "melee")
            {
                Data.EquippedItems[category] = "sword";
            }
            else
            {
                Data.EquippedItems[category] = null;
            }

            EmitSignal(SignalName.LoadoutChanged, category, Data.EquippedItems[category]);
            NotifyDataChanged();
            return true;
        }

        public bool IsUnlocked(string itemId)
        {
            var item = ItemCatalog.Instance?.Get(itemId);
            if (item == null) return false;

            return Data.UnlockedItems.ContainsKey(item.Category) &&
                   Data.UnlockedItems[item.Category].Contains(itemId);
        }

        public bool IsEquipped(string itemId)
        {
            var item = ItemCatalog.Instance?.Get(itemId);
            if (item == null) return false;

            return Data.EquippedItems.ContainsKey(item.Category) &&
                   Data.EquippedItems[item.Category] == itemId;
        }

        public string GetEquipped(string category)
        {
            if (!Data.EquippedItems.ContainsKey(category))
                return null;

            return Data.EquippedItems[category];
        }

        public string[] GetUnlocked(string category)
        {
            if (!Data.UnlockedItems.ContainsKey(category))
                return new string[0];

            return Data.UnlockedItems[category].ToArray();
        }
    }
}
