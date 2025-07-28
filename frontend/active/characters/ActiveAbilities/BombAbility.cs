using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.ActiveAbilities
{
    public partial class BombAbility : Node, IActiveAbility
    {
        [Export] private float _cooldownTime = 5.0f;
        [Export] private float _explosionRadius = 100.0f;
        [Export] private float _explosionDamage = 50.0f;
        [Export] private float _fuseTime = 3.0f;

        private Character _owner;
        private Timer _cooldownTimer;
        private PackedScene _bombScene;
        private Bomb _currentBomb;
        private ActiveAbilityPhase _currentPhase = ActiveAbilityPhase.Ready;
        private float _bombFuseTimer = 0f;
        private bool _bombDetonating = false;

        public override void _Ready()
        {
            _cooldownTimer = new Timer();
            _cooldownTimer.WaitTime = _cooldownTime;
            _cooldownTimer.OneShot = true;
            AddChild(_cooldownTimer);

            _bombScene = GD.Load<PackedScene>("res://active/characters/ActiveAbilities/bomb.tscn");
        }

        public void Initialize(Character owner)
        {
            _owner = owner;
        }

        public bool Activate()
        {
            if (_owner == null || _bombScene == null)
            {
                return false;
            }

            if (_currentBomb != null)
            {
                _currentBomb.ManualDetonate();
                _currentBomb = null;
                _bombDetonating = true;
                _currentPhase = ActiveAbilityPhase.InProgress;
                GD.Print("Bomb manually detonated!");
                return true;
            }

            if (IsOnCooldown())
            {
                return false;
            }

            var bomb = _bombScene.Instantiate<Bomb>();
            if (bomb != null)
            {
                _owner.GetParent().AddChild(bomb);
                bomb.Initialize(_owner.GlobalPosition, _explosionRadius, _explosionDamage, _fuseTime);
                bomb.BombDestroyed += OnBombDestroyed;
                _currentBomb = bomb;
                _bombFuseTimer = _fuseTime;
                _bombDetonating = false;
                _currentPhase = ActiveAbilityPhase.InProgress;
                GD.Print("Bomb dropped!");
                return true;
            }

            return false;
        }

        public bool IsOnCooldown()
        {
            return !_cooldownTimer.IsStopped();
        }

        public float GetCooldownRemaining()
        {
            return (float)_cooldownTimer.TimeLeft;
        }

        public ActiveAbilityPhase GetCurrentPhase()
        {
            if (_currentBomb != null || _bombDetonating)
            {
                return ActiveAbilityPhase.InProgress;
            }
            else if (IsOnCooldown())
            {
                return ActiveAbilityPhase.Cooldown;
            }
            else
            {
                return ActiveAbilityPhase.Ready;
            }
        }

        public float GetPhaseCompletionPercentage()
        {
            switch (_currentPhase)
            {
                case ActiveAbilityPhase.Loading:
                    return 0f;

                case ActiveAbilityPhase.InProgress:
                    if (_currentBomb == null && !_bombDetonating) return 1f;
                    if (_bombDetonating) return 1f;
                    if (_fuseTime <= 0f) return 1f;
                    return 1f - (_bombFuseTimer / _fuseTime);

                case ActiveAbilityPhase.Cooldown:
                    if (_cooldownTime <= 0f) return 1f;
                    return 1f - (GetCooldownRemaining() / _cooldownTime);

                case ActiveAbilityPhase.Ready:
                default:
                    return 1f;
            }
        }

        public override void _Process(double delta)
        {
            if (_currentBomb != null && !_bombDetonating)
            {
                _bombFuseTimer -= (float)delta;
                if (_bombFuseTimer <= 0f)
                {
                    _bombFuseTimer = 0f;
                }
            }
        }

        public void OnBombDestroyed()
        {
            _currentBomb = null;
            _bombDetonating = false;
            _currentPhase = ActiveAbilityPhase.Cooldown;
            _cooldownTimer.Start();
        }
    }
}