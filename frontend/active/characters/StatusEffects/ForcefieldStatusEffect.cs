using Godot;
using nuscutiesapp.active.characters.DamageSystem;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class ForcefieldStatusEffect : StatusEffect
    {
        private bool _hasBlocked = false;
        public bool HasBlocked => _hasBlocked;

        public override string StatusName => "Forcefield";

        public override void _Ready()
        {
            base._Ready();
            _duration = 30.0f;
            var forcefieldParticleScene = GD.Load<PackedScene>("res://active/characters/StatusEffects/particles/forcefield_particles.tscn");
            _visualEffectStrategy = new ParticleEffectStrategy(forcefieldParticleScene);
        }

        protected override void OnApplied()
        {
            GD.Print($"Applied forcefield to {_target.Name}");
        }

        protected override void OnRemoved()
        {
            GD.Print($"Removed forcefield from {_target.Name}");
        }

        public void BlockDamage()
        {
            if (!_hasBlocked && _isActive)
            {
                _hasBlocked = true;
                GD.Print("Forcefield absorbed a hit!");
                Remove();
            }
        }
    }
}