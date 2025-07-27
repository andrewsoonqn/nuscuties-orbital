using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.StatusEffects;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.PassiveEffects
{
    public partial class ReviveEffect : Node, IPassiveEffect
    {
        [Export] private float _reviveHealth = 10.0f;
        [Export] private float _forcefieldDuration = 5.0f;

        private HealthComponent _targetHealth;
        private bool _reviveUsed = false;
        public bool ReviveUsed => _reviveUsed;

        public override void _Ready()
        {
        }

        public void ApplyEffect(Node player)
        {
            if (player is Character character)
            {
                _targetHealth = character.GetNode<HealthComponent>("Health");
                if (_targetHealth != null)
                {
                    _reviveUsed = false;
                    GD.Print("Applied revive effect: Will revive once when health reaches 0");
                }
            }
        }

        public void RemoveEffect(Node player)
        {
            if (_targetHealth != null)
            {
                GD.Print("Removed revive effect");
            }
        }

        public void TriggerRevive(DamageInfo damageInfo)
        {
            if (_reviveUsed) return;

            var character = _targetHealth.GetParent<Character>();
            if (character == null) return;

            _reviveUsed = true;
            _targetHealth.CurrentHP = _reviveHealth;
            _targetHealth.TakeDamage(new DamageInfo(0, damageInfo.Knockback));

            if (character.StatusEffects != null)
            {
                var forcefieldEffect = new ForcefieldStatusEffect();
                character.StatusEffects.ApplyStatusEffect(forcefieldEffect);

                var timer = GetTree().CreateTimer(_forcefieldDuration);
                timer.Timeout += () =>
                {
                    if (forcefieldEffect.IsActive)
                    {
                        forcefieldEffect.Remove();
                    }
                };
            }

            GD.Print($"Revive effect triggered! Health restored to {_reviveHealth}, forcefield applied for {_forcefieldDuration} seconds");
        }
    }
}
