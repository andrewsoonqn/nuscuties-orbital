using nuscutiesapp.active.characters.StatusEffects;

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

    }
}