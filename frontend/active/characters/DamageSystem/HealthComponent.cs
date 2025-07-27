using Godot;
using nuscutiesapp.active.characters.StatusEffects;
using nuscutiesapp.tools;
using System;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class HealthComponent : Node, IDamageable
    {
        protected Character _owner;
        [Export] public float MaxHP = 20;
        public float CurrentHP { get; set; }

        public event Action<float, DamageInfo> Damaged;
        public event Action<DamageInfo> Died;

        protected DerivedStatCalculator _derivedStatCalculator;
        private Color _originalModulation;

        public override void _Ready()
        {
            _owner = GetParent<Character>();
            CurrentHP = MaxHP;
            _derivedStatCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        }

        protected virtual float GetDamageAmt(in DamageInfo damageInfo)
        {
            return damageInfo.Amount;
        }

        public virtual void TakeDamage(in DamageInfo damageInfo)
        {
            if (CurrentHP <= 0) return;

            bool forcefieldActive = false;
            if (_owner.StatusEffects != null)
            {
                var forcefieldEffect = _owner.StatusEffects.GetStatusEffect<ForcefieldStatusEffect>();
                if (forcefieldEffect != null && forcefieldEffect.TryBlockDamage())
                {
                    forcefieldActive = true;
                }
            }

            if (!forcefieldActive)
            {
                CurrentHP = float.Max(CurrentHP - GetDamageAmt(damageInfo), 0);
                ApplyDamageModulation();

                if (CurrentHP <= 0)
                {
                    if (!TryRevive(damageInfo))
                    {
                        Died?.Invoke(damageInfo);
                    }
                }
                else
                {
                    Damaged?.Invoke(CurrentHP, damageInfo);
                }
            }
            else
            {
                Damaged?.Invoke(CurrentHP, damageInfo);
            }
        }

        protected virtual bool TryRevive(in DamageInfo damageInfo)
        {
            return false;
        }

        private async void ApplyDamageModulation()
        {
            if (_owner?.AnimatedSprite != null)
            {
                _originalModulation = _owner.AnimatedSprite.Modulate;
                _owner.AnimatedSprite.Modulate = new Color(1, 0.5f, 0.5f);

                await _owner.ToSignal(_owner.GetTree().CreateTimer(0.3f), SceneTreeTimer.SignalName.Timeout);

                if (_owner?.AnimatedSprite != null)
                {
                    _owner.AnimatedSprite.Modulate = _originalModulation;
                }
            }
        }
    }
}
