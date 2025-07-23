using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.ui.shop
{
    public partial class ItemCard : PanelContainer
    {
        [Export] private TextureRect _iconTexture;
        [Export] private Label _titleLabel;
        [Export] private RichTextLabel _descriptionLabel;
        [Export] private Button _actionButton;

        private ItemDef _item;
        private PlayerInventoryManager _inventoryManager;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");

            if (_actionButton != null)
            {
                _actionButton.Pressed += OnActionButtonPressed;
            }
        }

        public void SetupCard(ItemDef item)
        {
            _item = item;
            UpdateDisplay();
            UpdateState();
        }

        private void UpdateDisplay()
        {
            if (_item == null) return;

            _titleLabel.Text = _item.Title;
            _descriptionLabel.Text = _item.Description;

            if (!string.IsNullOrEmpty(_item.IconPath))
            {
                var texture = GD.Load<Texture2D>(_item.IconPath);
                if (texture != null)
                {
                    _iconTexture.Texture = texture;
                }
            }
        }

        private void UpdateState()
        {
            if (_item == null || _inventoryManager == null) return;

            bool isUnlocked = _inventoryManager.IsUnlocked(_item.Id);
            bool isEquipped = _inventoryManager.IsEquipped(_item.Id);
            bool canAfford = _inventoryManager.Coins >= _item.Price;

            if (isEquipped)
            {
                _actionButton.Text = "Unequip";
                _actionButton.Disabled = false;
                Modulate = new Color(0.8f, 1.0f, 0.8f);
            }
            else if (isUnlocked)
            {
                _actionButton.Text = "Equip";
                _actionButton.Disabled = false;
                Modulate = new Color(1.0f, 1.0f, 1.0f);
            }
            else
            {
                _actionButton.Text = $"Buy for {_item.Price} coins";
                _actionButton.Disabled = !canAfford;
                Modulate = canAfford ? new Color(1.0f, 1.0f, 1.0f) : new Color(0.7f, 0.7f, 0.7f);
            }
        }

        private void OnActionButtonPressed()
        {
            if (_item == null || _inventoryManager == null) return;

            bool isUnlocked = _inventoryManager.IsUnlocked(_item.Id);
            bool isEquipped = _inventoryManager.IsEquipped(_item.Id);

            if (isEquipped)
            {
                _inventoryManager.UnequipItem(_item.Category);
            }
            else if (isUnlocked)
            {
                _inventoryManager.EquipItem(_item.Id);
            }
            else
            {
                _inventoryManager.UnlockItem(_item.Id);
            }

            UpdateState();
        }

        public void OnInventoryChanged()
        {
            UpdateState();
        }

        public void OnCoinsChanged()
        {
            UpdateState();
        }

        public override void _ExitTree()
        {
            if (_actionButton != null)
            {
                _actionButton.Pressed -= OnActionButtonPressed;
            }
        }
    }
}
