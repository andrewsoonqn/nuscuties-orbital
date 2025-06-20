using Godot;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly Vector2 Knockback;

        public DamageInfo(float amount, Vector2 knockback)
        {
            Amount = amount;
            Knockback = knockback;
        }
    }
}