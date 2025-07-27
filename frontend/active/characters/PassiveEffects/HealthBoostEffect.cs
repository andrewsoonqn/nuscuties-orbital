using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.PassiveEffects
{
    public partial class HealthBoostEffect : Node, IPassiveEffect
    {
        [Export] private float _healthBonus = 20.0f;
        [Export] private float _regenTickAmount = 5.0f;
        [Export] private float _regenTickInterval = 1.0f;
        [Export] private float _regenDelay = 3.0f;

        private float _originalMaxHP;
        private HealthComponent _targetHealth;
        private float _lastDamageTime;
        private float _lastRegenTickTime;
        private bool _isRegenerating = false;

        public override void _Ready()
        {
            _lastDamageTime = Time.GetTicksMsec() / 1000.0f; // Initialize with current time in seconds
            _lastRegenTickTime = _lastDamageTime;
        }

        public override void _Process(double delta)
        {
            if (_targetHealth == null || _targetHealth.CurrentHP >= _targetHealth.MaxHP)
                return;

            float currentTime = (float)Time.GetTicksMsec() / 1000.0f; // Get current time in seconds
            float timeSinceLastDamage = currentTime - _lastDamageTime;

            if (timeSinceLastDamage >= _regenDelay && !_isRegenerating)
            {
                _isRegenerating = true;
                _lastRegenTickTime = currentTime;
                GD.Print("Health regeneration started");
            }

            if (_isRegenerating)
            {
                float timeSinceLastTick = currentTime - _lastRegenTickTime;

                if (timeSinceLastTick >= _regenTickInterval)
                {
                    _targetHealth.CurrentHP = Mathf.Min(_targetHealth.CurrentHP + _regenTickAmount, _targetHealth.MaxHP);
                    _lastRegenTickTime = currentTime;
                    GD.Print($"Health regeneration tick: +{_regenTickAmount} HP (current: {_targetHealth.CurrentHP}/{_targetHealth.MaxHP})");
                }
            }
        }

        public void ApplyEffect(Node player)
        {
            if (player is Character character)
            {
                _targetHealth = character.GetNode<HealthComponent>("Health");
                if (_targetHealth != null)
                {
                    _originalMaxHP = _targetHealth.MaxHP;
                    _targetHealth.MaxHP += _healthBonus;

                    _targetHealth.Damaged += OnDamaged;

                    GD.Print($"Applied health boost: +{_healthBonus} HP (new max: {_targetHealth.MaxHP})");
                    GD.Print($"Health regeneration enabled: {_regenTickAmount} HP every {_regenTickInterval} seconds after {_regenDelay} seconds of no damage");
                }
            }
        }

        public void RemoveEffect(Node player)
        {
            if (_targetHealth != null)
            {
                _targetHealth.Damaged -= OnDamaged;
                _targetHealth.MaxHP = _originalMaxHP;
                GD.Print($"Removed health boost (max HP restored to: {_originalMaxHP})");
            }
        }

        private void OnDamaged(float newHp, DamageInfo info)
        {
            _lastDamageTime = (float)Time.GetTicksMsec() / 1000;
            _isRegenerating = false;
            GD.Print("Health regeneration interrupted by damage");
        }
    }
}
