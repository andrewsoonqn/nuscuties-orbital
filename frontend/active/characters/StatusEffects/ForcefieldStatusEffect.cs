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
            _duration = float.MaxValue;
            var shieldScene = GD.Load<PackedScene>("res://active/characters/StatusEffects/shield_overlay.tscn");

            _visualEffectStrategy = new SpriteOverlayStrategy(
                shieldScene
            );
        }

        protected override void OnApplied()
        {
            GD.Print($"Applied forcefield to {_target.Name}");
        }

        protected override void OnRemoved()
        {
            GD.Print($"Removed forcefield from {_target.Name}");
        }

        public bool TryBlockDamage()
        {
            if (!_hasBlocked && _isActive)
            {
                _hasBlocked = true;
                GD.Print("Forcefield absorbed damage but allowed knockback!");
                Remove();
                return true;
            }
            return false;
        }
    }
}