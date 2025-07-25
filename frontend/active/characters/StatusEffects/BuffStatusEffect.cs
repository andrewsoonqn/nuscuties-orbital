using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class BuffStatusEffect : StatusEffect
    {
        [Export] private float _speedMultiplier = 1.5f;
        [Export] private float _sizeMultiplier = 1.2f;
        [Export] private float _damageMultiplier = 1.3f;

        private float _originalMaxSpeed;
        private Vector2 _originalScale;
        private bool _statsModified = false;
        private DerivedStatCalculator _statCalculator;

        public override string StatusName => "Buff";

        public override void _Ready()
        {
            base._Ready();
            _duration = 10.0f;
            _statCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        }

        protected override void OnApplied()
        {
            if (_target != null)
            {
                _originalMaxSpeed = _target.MaxSpeed;
                _originalScale = _target.Scale;

                _target.MaxSpeed = _originalMaxSpeed * _speedMultiplier;
                _target.Scale = _originalScale * _sizeMultiplier;
                _statsModified = true;

                GD.Print($"Applied buff to {_target.Name}: Speed x{_speedMultiplier}, Size x{_sizeMultiplier}, Damage x{_damageMultiplier}");
                ApplyGoldModulation();
            }
        }

        protected override void OnRemoved()
        {
            if (_target != null && _statsModified)
            {
                _target.MaxSpeed = _originalMaxSpeed;
                _target.Scale = _originalScale;
                GD.Print($"Removed buff from {_target.Name}");

                if (_target.AnimatedSprite != null)
                {
                    _target.AnimatedSprite.Modulate = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }

        private void ApplyGoldModulation()
        {
            if (_target?.AnimatedSprite != null)
            {
                _target.AnimatedSprite.Modulate = new Color(1.2f, 1.0f, 0.7f);
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (_isActive && _target?.AnimatedSprite != null)
            {
                ApplyGoldModulation();
            }
        }

        public float GetDamageMultiplier()
        {
            return _isActive ? _damageMultiplier : 1.0f;
        }
    }
}