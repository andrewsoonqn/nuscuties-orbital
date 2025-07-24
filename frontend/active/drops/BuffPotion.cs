using Godot;
using nuscutiesapp.active.characters.StatusEffects;

namespace nuscutiesapp.active.drops
{
    public partial class BuffPotion : BaseDropItem
    {
        protected override void OnPickup(Player player)
        {
            if (player.StatusEffects != null)
            {
                player.StatusEffects.ApplyStatusEffect<BuffStatusEffect>();
            }
        }
    }
}
