using Godot;

namespace nuscutiesapp.tools
{
    public partial class DerivedStatCalculator : Node
    {
        private const float _strBonusPerPoint = 0.03f;
        private const int _staReductionPerPoint = 2;
        private const int _HPIncreasePerLevel = 10;
        private const int _baseHP = 60;
        private const int _damageIncreasePerLevel = 5;
        private const int _baseDamage = 20;
        
        private ProgressionManager _progressionManager;
        private PlayerStatManager _playerStatManager;

        public override void _Ready()
        {
            _playerStatManager = GetNode<PlayerStatManager>("/root/PlayerStatManager");
            _progressionManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        }

        public float CalcAttackDamageMultiplier()
        {
            return 1 + _playerStatManager.GetStrength() * _strBonusPerPoint;
        }

        public double CalcDamageReduction()
        {
            return _playerStatManager.GetStamina() * _staReductionPerPoint;
        }

        public double CalcMaxHP()
        {
            return _baseHP + (_progressionManager.GetLevel() * _HPIncreasePerLevel);
        }

        public double CalcDamage()
        {
            return _baseDamage + (_progressionManager.GetLevel() * _damageIncreasePerLevel);
        }
    }
}