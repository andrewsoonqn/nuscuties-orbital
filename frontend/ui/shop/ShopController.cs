using Godot;
using nuscutiesapp.tools;
using System.Collections.Generic;
using System.Linq;

namespace nuscutiesapp.ui.shop
{
    public partial class ShopController : Control
    {
        private Button _backButton;
        private Label _coinsLabel;
        private TabContainer _tabContainer;

        private Dictionary<string, GridContainer> _categoryGrids;
        private List<ItemCard> _itemCards = new();

        private PlayerInventoryManager _inventoryManager;
        private ItemCatalog _itemCatalog;

        private PackedScene _itemCardScene;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
            _itemCatalog = GetNode<ItemCatalog>("/root/ItemCatalog");

            _itemCardScene = GD.Load<PackedScene>("res://ui/shop/item_card.tscn");

            SetupUI();
            ConnectSignals();
            PopulateShop();
            UpdateCoinsDisplay();
        }

        private void SetupUI()
        {
            _backButton = GetNode<Button>("VBoxContainer/Header/BackButton");
            _coinsLabel = GetNode<Label>("VBoxContainer/Header/CoinsDisplay/CoinsLabel");
            _tabContainer = GetNode<TabContainer>("VBoxContainer/TabContainer");

            _categoryGrids = new Dictionary<string, GridContainer>
            {
                { "melee", GetNode<GridContainer>("VBoxContainer/TabContainer/Melee/MeleeGrid") },
                { "projectile", GetNode<GridContainer>("VBoxContainer/TabContainer/Projectile/ProjectileGrid") },
                { "utility", GetNode<GridContainer>("VBoxContainer/TabContainer/Utility/UtilityGrid") },
                { "necklace_passive", GetNode<GridContainer>("VBoxContainer/TabContainer/Passive Necklaces/PassiveGrid") },
                { "necklace_active", GetNode<GridContainer>("VBoxContainer/TabContainer/Active Necklaces/ActiveGrid") }
            };
        }

        private void ConnectSignals()
        {
            _backButton.Pressed += OnBackButtonPressed;

            if (_inventoryManager != null)
            {
                _inventoryManager.CoinsChanged += OnCoinsChanged;
                _inventoryManager.InventoryChanged += OnInventoryChanged;
                _inventoryManager.LoadoutChanged += OnLoadoutChanged;
            }
        }

        private void PopulateShop()
        {
            if (_itemCatalog == null)
            {
                GD.PrintErr("ItemCatalog instance not found");
                return;
            }

            ClearAllItems();

            foreach (string category in _categoryGrids.Keys)
            {
                var items = _itemCatalog.GetByCategory(category);
                PopulateCategory(category, items.ToArray());
            }
        }

        private void PopulateCategory(string category, ItemDef[] items)
        {
            if (!_categoryGrids.TryGetValue(category, out GridContainer grid))
            {
                GD.PrintErr($"Grid not found for category: {category}");
                return;
            }

            foreach (var item in items)
            {
                var cardInstance = _itemCardScene.Instantiate<ItemCard>();
                grid.AddChild(cardInstance);
                cardInstance.SetupCard(item);
                _itemCards.Add(cardInstance);
            }
        }

        private void ClearAllItems()
        {
            foreach (var card in _itemCards)
            {
                card?.QueueFree();
            }
            _itemCards.Clear();

            foreach (var grid in _categoryGrids.Values)
            {
                foreach (Node child in grid.GetChildren())
                {
                    child.QueueFree();
                }
            }
        }

        private void OnBackButtonPressed()
        {
            GetTree().ChangeSceneToFile(Paths.Home);
        }

        private void OnCoinsChanged(int newTotal)
        {
            UpdateCoinsDisplay();
            RefreshAllCards();
        }

        private void OnInventoryChanged(string category, string[] unlockedIds)
        {
            RefreshAllCards();
        }

        private void OnLoadoutChanged(string category, string equippedId)
        {
            RefreshAllCards();
        }

        private void UpdateCoinsDisplay()
        {
            if (_inventoryManager != null && _coinsLabel != null)
            {
                _coinsLabel.Text = $"{_inventoryManager.Coins} Coins";
            }
        }

        private void RefreshAllCards()
        {
            foreach (var card in _itemCards)
            {
                card.OnInventoryChanged();
                card.OnCoinsChanged();
            }
        }

        public override void _ExitTree()
        {
            if (_inventoryManager != null)
            {
                _inventoryManager.CoinsChanged -= OnCoinsChanged;
                _inventoryManager.InventoryChanged -= OnInventoryChanged;
                _inventoryManager.LoadoutChanged -= OnLoadoutChanged;
            }
        }
    }
}