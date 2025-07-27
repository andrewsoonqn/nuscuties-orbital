using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.ActiveAbilities
{
    public partial class DualWieldAbility : Node, IActiveAbility
    {
        [Export] private float _cooldownTime = 0.5f;
        [Export] private bool _isActive = false;

        private Character _owner;
        private Timer _cooldownTimer;
        private PackedScene _secondHandScene;
        private SecondHand _secondHand;
        private Player _player;

        public override void _Ready()
        {
            _cooldownTimer = new Timer();
            _cooldownTimer.WaitTime = _cooldownTime;
            _cooldownTimer.OneShot = true;
            AddChild(_cooldownTimer);

            _secondHandScene = GD.Load<PackedScene>("res://active/characters/SecondHand.tscn");
        }

        public void Initialize(Character owner)
        {
            _owner = owner;
            _player = owner as Player;
        }

        public bool Activate()
        {
            if (_owner == null || _secondHandScene == null)
            {
                return false;
            }

            if (IsOnCooldown())
            {
                return false;
            }

            if (!_isActive)
            {
                EnableDualWield();
            }
            else
            {
                DisableDualWield();
            }

            _cooldownTimer.Start();
            return true;
        }

        private void EnableDualWield()
        {
            if (_secondHand == null)
            {
                _secondHand = _secondHandScene.Instantiate<SecondHand>();
                _owner.AddChild(_secondHand);
                _secondHand.Initialize(_owner, _owner.MyWeapon);
            }

            _secondHand.SetVisible(true);
            _isActive = true;
            GD.Print("Dual wield enabled!");
        }

        private void DisableDualWield()
        {
            if (_secondHand != null)
            {
                _secondHand.SetVisible(false);
            }
            _isActive = false;
            GD.Print("Dual wield disabled!");
        }

        public void OnPlayerAttack()
        {
            if (_isActive && _secondHand != null)
            {
                _secondHand.Attack();
            }
        }

        public void OnPlayerWeaponUpdate()
        {
            if (_isActive && _secondHand != null && _owner.MyWeapon != null)
            {
                _secondHand.UpdateWeapon(_owner.MyWeapon);
            }
        }

        public void OnPlayerWeaponRotation(Vector2 direction)
        {
            if (_isActive && _secondHand != null)
            {
                _secondHand.UpdateWeaponRotation(direction);
            }
        }

        public bool IsOnCooldown()
        {
            return !_cooldownTimer.IsStopped();
        }

        public float GetCooldownRemaining()
        {
            return (float)_cooldownTimer.TimeLeft;
        }

        public bool IsActive()
        {
            return _isActive;
        }
    }
}
