using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.PassiveEffects
{
    public partial class HealthBoostEffect : Node, IPassiveEffect
    {
        [Export] private float _healthBonus = 20.0f;
        private float _originalMaxHP;
        private HealthComponent _targetHealth;

        public void ApplyEffect(Node player)
        {
            if (player is Character character)
            {
                _targetHealth = character.GetNode<HealthComponent>("Health");
                if (_targetHealth != null)
                {
                    _originalMaxHP = _targetHealth.MaxHP;
                    _targetHealth.MaxHP += _healthBonus;
                    GD.Print($"Applied health boost: +{_healthBonus} HP (new max: {_targetHealth.MaxHP})");
                }
            }
        }

        public void RemoveEffect(Node player)
        {
            if (_targetHealth != null)
            {
                _targetHealth.MaxHP = _originalMaxHP;
                GD.Print($"Removed health boost (max HP restored to: {_originalMaxHP})");
            }
        }
    }
}