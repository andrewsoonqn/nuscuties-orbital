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
        }

        protected override void OnApplied()
        {
            GD.Print($"Applied forcefield to {_target.Name}");
            ApplyShieldModulation();
        }

        protected override void OnRemoved()
        {
            GD.Print($"Removed forcefield from {_target.Name}");

            if (_target.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
            }
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

        private void ApplyShieldModulation()
        {
            if (_target?.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(0.7f, 0.9f, 1.2f);
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (_isActive && _target?.AnimatedSprite != null)
            {
                ApplyShieldModulation();
            }
        }
    }
}
