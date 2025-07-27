using nuscutiesapp.active.characters.StatusEffects;
using nuscutiesapp.active.characters.PassiveEffects;
using System.Linq;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class PlayerHealthComponent : HealthComponent
    {
        private ProgressionManager _progressionManager;
        public override void _Ready()
        {
            base._Ready();
            _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
            _progressionManager.LeveledUp += ProgressionManagerOnLeveledUp;
            MaxHP = _derivedStatCalculator.CalcMaxHP();
            CurrentHP = MaxHP;
        }

        private void ProgressionManagerOnLeveledUp(int level, int extraLevels)
        {
            MaxHP = _derivedStatCalculator.CalcMaxHP();
        }

        protected override float GetDamageAmt(in DamageInfo damageInfo)
        {
            float damageAmt = damageInfo.Amount - _derivedStatCalculator.CalcDamageReduction();
            damageAmt = float.Max(damageAmt, 0);
            return damageAmt;
        }

        protected override bool TryRevive(in DamageInfo damageInfo)
        {
            if (_owner == null) return false;

            var reviveEffects = _owner.GetChildren().OfType<ReviveEffect>().Where(e => !e.ReviveUsed).ToList();
            if (reviveEffects.Count > 0)
            {
                var reviveEffect = reviveEffects.First();
                reviveEffect.TriggerRevive(damageInfo);
                return true;
            }
            return false;
        }

    }
}
