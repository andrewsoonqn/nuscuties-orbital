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
                _cooldownTimer.Start();
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

        public void OnBombDestroyed()
        {
            _currentBomb = null;
            _cooldownTimer.Start();
        }
    }
}
