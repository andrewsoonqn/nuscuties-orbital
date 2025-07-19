using Godot;
using System.Collections.Generic;
using System.IO;

namespace nuscutiesapp.tools
{
    public partial class PlayerInventoryManager : BaseStatManager<PlayerInventoryData>
    {
        public override void _Ready()
        {
            GD.Print(OS.GetUserDataDir());
            base._Ready();
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
