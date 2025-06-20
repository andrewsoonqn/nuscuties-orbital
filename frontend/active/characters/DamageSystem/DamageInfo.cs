using Godot;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly Vector2 Knockback;
        public readonly Character Attacker;
        public readonly DamageType Type; // (enum) melee, projectile

        public DamageInfo(float amount, Vector2 knockback, Character attacker,
            int type)
        {
            Amount = amount;
            Knockback = knockback;
            Attacker = attacker;
            Type = type;
        }
    }
}