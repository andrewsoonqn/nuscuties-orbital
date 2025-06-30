using Godot;

namespace nuscutiesapp.tools
{
    public partial class DerivedStatCalculator : Node
    {
        private const float _strBonusPerPoint = 0.02f;
        private const float _staReductionPerPoint = 0.5f;
        private const int _HPIncreasePerLevel = 5;
        private const int _baseHP = 60;
        private const float _damageIncreasePerLevel = 2.5f;
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

        public float CalcDamageReduction()
        {
            return _playerStatManager.GetStamina() * _staReductionPerPoint;
        }

        public float CalcMaxHP()
        {
            return _baseHP + ((_progressionManager.GetLevel() - 1) * _HPIncreasePerLevel);
        }

        public float CalcDamage()
        {
            return _baseDamage + ((_progressionManager.GetLevel() - 1) * _damageIncreasePerLevel);
        }
    }
}