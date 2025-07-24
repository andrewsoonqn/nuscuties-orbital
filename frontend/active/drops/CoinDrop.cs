using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.drops
{
    public partial class CoinDrop : BaseDropItem
    {
        private int _coinValue;
        private PlayerInventoryManager _inventoryManager;

        public override void _Ready()
        {
            _inventoryManager = GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
            base._Ready();
        }

        public void Initialize(int coinValue)
        {
            _coinValue = coinValue;
        }

        protected override void OnPickup(Player player)
        {
            _inventoryManager.AddCoins(_coinValue);

            var numberManager = GetNode<BaseNumberManager>("/root/BaseNumberManager");
            if (numberManager != null)
            {
                numberManager.ShowCoinGain(_coinValue, player.Position, GetParent<Node2D>());
            }
        }
    }
}
