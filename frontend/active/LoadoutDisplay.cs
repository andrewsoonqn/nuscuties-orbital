using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active
{
    public partial class LoadoutDisplay : Control
    {
        [Export] private TextureRect _meleeWeaponIcon;
        [Export] private TextureRect _projectileWeaponIcon;
        [Export] private TextureRect _activeAbilityIcon;
        [Export] private TextureRect _passiveEffectIcon;

        private PlayerInventoryManager _inventoryManager;
        private ItemCatalog _itemCatalog;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
            _itemCatalog = GetNode<ItemCatalog>("/root/ItemCatalog");

            UpdateLoadoutDisplay();
        }

        public void UpdateLoadoutDisplay()
        {
            UpdateWeaponIcon(_meleeWeaponIcon, "melee");
            UpdateWeaponIcon(_projectileWeaponIcon, "projectile");
            UpdateWeaponIcon(_activeAbilityIcon, "necklace_active");
            UpdateWeaponIcon(_passiveEffectIcon, "necklace_passive");
        }

        private void UpdateWeaponIcon(TextureRect iconRect, string category)
        {
            if (iconRect == null) return;

            string equippedItemId = _inventoryManager.GetEquipped(category);

            if (string.IsNullOrEmpty(equippedItemId))
            {
                var noItemTexture = GD.Load<Texture2D>("res://assets/item_icons/no_item.png");
                iconRect.Texture = noItemTexture;
                return;
            }

            var itemDef = _itemCatalog.Get(equippedItemId);
            if (itemDef == null)
            {
                GD.PrintErr($"Item not found in catalog: {equippedItemId}");
                var noItemTexture = GD.Load<Texture2D>("res://assets/item_icons/no_item.png");
                iconRect.Texture = noItemTexture;
                return;
            }

            if (!string.IsNullOrEmpty(itemDef.IconPath))
            {
                var texture = GD.Load<Texture2D>(itemDef.IconPath);
                if (texture != null)
                {
                    iconRect.Texture = texture;
                }
                else
                {
                    GD.PrintErr($"Failed to load texture: {itemDef.IconPath}");
                    var noItemTexture = GD.Load<Texture2D>("res://assets/item_icons/no_item@no_item.png");
                    iconRect.Texture = noItemTexture;
                }
            }
            else
            {
                var noItemTexture = GD.Load<Texture2D>("res://assets/item_icons/no_item@no_item.png");
                iconRect.Texture = noItemTexture;
            }
        }
    }
}
