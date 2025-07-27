using Godot;
using nuscutiesapp.active.characters.DamageSystem;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class BurnStatusEffect : StatusEffect
    {
        [Export] private float _damagePerTick = 5.0f;

        public override string StatusName => "Burn";

        public override void _Ready()
        {
            base._Ready();
            var burnParticleScene = GD.Load<PackedScene>("res://active/characters/StatusEffects/particles/burn_particles.tscn");
            _visualEffectStrategy = new ParticleEffectStrategy(burnParticleScene);
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