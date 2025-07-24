using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.ui;

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

                var numberManager = GetNode<DamageNumberManager>("/root/DamageNumberManager");
                if (numberManager != null)
                {
                    numberManager.ShowHealthGain(actualHeal, GlobalPosition, GetParent<Node2D>());
                }
            }
        }
    }
}
