namespace nuscutiesapp.tools
{
    public class DerivedStatCalculator
    {
        private const double _strBonusPerPoint = 0.03;
        private const int _staReductionPerPoint = 2;
        private const int _HPIncreasePerLevel = 10;
        private const int _baseHP = 60;
        private const int _damageIncreasePerLevel = 5;
        private const int _baseDamage = 20;

        public double CalcAttackDamageMultiplier(int strength)
        {
            return 1 + strength * _strBonusPerPoint;
        }
        
        public double CalcDamageReduction(int stamina)
        {
            return stamina * _staReductionPerPoint;
        }

        public double CalcMaxHP(int level)
        {
            return _baseHP + (level * _HPIncreasePerLevel);
        }

        public double CalcDamage(int level)
        {
            return _baseDamage + (level * _damageIncreasePerLevel);
        }
    }
}