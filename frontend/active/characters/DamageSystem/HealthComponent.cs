using Godot;
using nuscutiesapp.tools;
using System;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class HealthComponent : Node, IDamageable
    {
        protected Character _owner; // can change to not just character in the future
        [Export] public float MaxHP = 20; // TODO make this private
        public float CurrentHP { get; set; }

        public event Action<float, DamageInfo> Damaged;
        public event Action<DamageInfo> Died;

        protected DerivedStatCalculator _derivedStatCalculator;

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

            CurrentHP = float.Max(CurrentHP - GetDamageAmt(damageInfo), 0);

            ApplyDamageModulation();

            if (CurrentHP <= 0)
            {
                Died?.Invoke(damageInfo);
            }
            else
            {
                Damaged?.Invoke(CurrentHP, damageInfo);
            }
        }

        private async void ApplyDamageModulation()
        {
            _owner.AnimatedSprite.Modulate = new Color(1, 0.5f, 0.5f);

            await _owner.ToSignal(_owner.GetTree().CreateTimer(0.3f), SceneTreeTimer.SignalName.Timeout);

            _owner.AnimatedSprite.Modulate = new Color(1, 1, 1, 1);
        }
    }
}