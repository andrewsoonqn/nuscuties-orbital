using Godot;
using nuscutiesapp.tools;
using System;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class HealthComponent : Node, IDamageable
    {
        private Character _owner; // can change to not just character in the future
        [Export] public float MaxHP = 20;
        public float CurrentHP { get; private set; }

        public event Action<float, DamageInfo> Damaged;
        public event Action<DamageInfo> Died;

        private DerivedStatCalculator _derivedStatCalculator;

        public override void _Ready()
        {
            _owner = GetParent<Character>();
            CurrentHP = MaxHP;
            _derivedStatCalculator = GetNode<DerivedStatCalculator>("/root/DerivedStatCalculator");
        }

        public void TakeDamage(in DamageInfo damageInfo)
        {
            if (CurrentHP <= 0) return;

            float damageAmt;
            if (_owner is Player)
            {
                damageAmt = damageInfo.Amount - _derivedStatCalculator.CalcDamageReduction();
                damageAmt = float.Max(damageAmt, 0);
            }
            else
            {
                damageAmt = damageInfo.Amount;
            }
            
            CurrentHP -= damageAmt;

            Damaged?.Invoke(CurrentHP, damageInfo);
            if (CurrentHP <= 0)
            {
                Died?.Invoke(damageInfo);
            }
        }
    }
}