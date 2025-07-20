using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class SlowStatusEffect : StatusEffect
    {
        [Export] private float _speedMultiplier = 0.5f;

        private float _originalMaxSpeed;
        private bool _speedModified = false;

        public override string StatusName => "Slow";

        protected override void OnApplied()
        {
            if (_target != null)
            {
                var maxSpeedField = _target.GetType().GetField("_maxSpeed",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (maxSpeedField != null)
                {
                    _originalMaxSpeed = (float)maxSpeedField.GetValue(_target);
                    float newMaxSpeed = _originalMaxSpeed * _speedMultiplier;
                    maxSpeedField.SetValue(_target, newMaxSpeed);
                    _speedModified = true;

                    GD.Print($"Slowed {_target.Name}: {_originalMaxSpeed} -> {newMaxSpeed}");
                }

                if (_target.AnimatedSprite != null)
                {
                    _target.AnimatedSprite.Modulate = new Color(0.7f, 0.7f, 1.0f);
                }
            }
        }

        protected override void OnRemoved()
        {
            if (_target != null && _speedModified)
            {
                var maxSpeedField = _target.GetType().GetField("_maxSpeed",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (maxSpeedField != null)
                {
                    maxSpeedField.SetValue(_target, _originalMaxSpeed);
                    GD.Print($"Restored speed for {_target.Name}: {_originalMaxSpeed}");
                }

                if (_target.AnimatedSprite != null)
                {
                    _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }
}
