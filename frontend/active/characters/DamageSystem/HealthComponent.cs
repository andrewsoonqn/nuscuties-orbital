using Godot;
using System;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class HealthComponent : Node, IDamageable
    {
        [Export] public float MaxHP = 20;
        public float CurrentHP { get; private set; }

        public event Action<float, DamageInfo> Damaged;
        public event Action<DamageInfo> Died;

        public override void _Ready()
        {
            CurrentHP = MaxHP;
        }

        public void TakeDamage(in DamageInfo damageInfo)
        {
            if (CurrentHP <= 0) return;
            CurrentHP -= damageInfo.Amount;

            Damaged?.Invoke(CurrentHP, damageInfo);
            if (CurrentHP <= 0)
            {
                Died?.Invoke(damageInfo);
            }
        }
    }
}