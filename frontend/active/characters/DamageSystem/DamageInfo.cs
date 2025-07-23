using Godot;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly Vector2 Knockback;
        public readonly string StatusEffect;

        public DamageInfo(float amount, Vector2 knockback, string statusEffect = null)
        {
            Amount = amount;
            Knockback = knockback;
            StatusEffect = statusEffect;
        }
    }
}
