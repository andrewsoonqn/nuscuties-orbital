using Godot;
using nuscutiesapp.active.characters.DamageSystem;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class BurnStatusEffect : StatusEffect
    {
        [Export] private float _damagePerTick = 5.0f;

        public override string StatusName => "Burn";

        protected override void OnApplied()
        {
            if (_target?.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(1.0f, 0.7f, 0.7f);
            }
        }

        protected override void OnRemoved()
        {
            if (_target?.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
            }
        }

        protected override void OnTick()
        {
            if (_target?.Health != null)
            {
                var damageInfo = new DamageInfo(_damagePerTick, Vector2.Zero);
                _target.TakeDamage(damageInfo);

                GD.Print($"Burn damage: {_damagePerTick} to {_target.Name}");
            }
        }
    }
}