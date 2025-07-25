using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.drops
{
    public partial class HealthPotion : BaseDropItem
    {
        private float _healAmount = 25.0f;

        protected override void OnPickup(Player player)
        {
            if (player.Health.CurrentHP < player.Health.MaxHP)
            {
                float currentHP = player.Health.CurrentHP;
                float maxHP = player.Health.MaxHP;
                float actualHeal = Mathf.Min(_healAmount, maxHP - currentHP);

                player.Health.CurrentHP = Mathf.Min(currentHP + actualHeal, maxHP);

                var numberManager = GetNode<BaseNumberManager>("/root/BaseNumberManager");
                if (numberManager != null)
                {
                    numberManager.ShowHealthGain(actualHeal, Position, GetParent<Node2D>());
                }
            }
        }
    }
}
