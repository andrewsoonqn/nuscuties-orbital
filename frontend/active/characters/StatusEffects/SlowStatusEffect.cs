using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class SlowStatusEffect : StatusEffect
    {
        [Export] private float _speedMultiplier = 0.5f;

        private float _originalMaxSpeed;
        private bool _speedModified = false;

        public override string StatusName => "Slow";
        
        public override void _Process(double delta)
        {
            base._Process(delta);

            // Continuously maintain blue modulation while active
            if (_isActive && _target?.AnimatedSprite != null)
            {
                ApplyBlueModulation();
            }
        }

        protected override void OnApplied()
        {
            if (_target != null)
            {
                _originalMaxSpeed = _target.MaxSpeed;
                float newMaxSpeed = _originalMaxSpeed * _speedMultiplier;
                _target.MaxSpeed = newMaxSpeed;
                _speedModified = true;

                GD.Print($"Slowed {_target.Name}: {_originalMaxSpeed} -> {newMaxSpeed}");
                ApplyBlueModulation();
            }
        }

        protected override void OnRemoved()
        {
            if (_target != null && _speedModified)
            {
                _target.MaxSpeed = _originalMaxSpeed;
                GD.Print($"Restored speed for {_target.Name}: {_originalMaxSpeed}");

                if (_target.AnimatedSprite != null)
                {
                    _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }

        private void ApplyBlueModulation()
        {
            if (_target?.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(0.7f, 0.7f, 1.0f);
            }
        }
    }
}
