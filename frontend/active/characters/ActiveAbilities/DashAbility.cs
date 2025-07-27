using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.ActiveAbilities
{
    public partial class DashAbility : Node, IActiveAbility
    {
        [Export] private float _dashDistance = 60.0f;
        [Export] private float _cooldownTime = 3.0f;
        [Export] private float _dashDuration = 0.2f;

        private Character _owner;
        private Timer _cooldownTimer;
        private bool _isDashing = false;
        private Vector2 _dashDirection;
        private float _dashTimer = 0.0f;
        private ActiveAbilityPhase _currentPhase = ActiveAbilityPhase.Ready;

        public bool IsDashing => _isDashing;

        public override void _Ready()
        {
            _cooldownTimer = new Timer();
            _cooldownTimer.WaitTime = _cooldownTime;
            _cooldownTimer.OneShot = true;
            AddChild(_cooldownTimer);
        }

        public void Initialize(Character owner)
        {
            _owner = owner;
        }

        public bool Activate()
        {
            if (IsOnCooldown() || _isDashing || _owner == null)
            {
                return false;
            }

            _dashDirection = _owner.MovDirection;
            if (_dashDirection == Vector2.Zero)
            {
                _dashDirection = Vector2.Right;
            }

            _isDashing = true;
            _dashTimer = _dashDuration;
            _currentPhase = ActiveAbilityPhase.InProgress;

            float dashSpeed = _dashDistance / _dashDuration;
            _owner.MaxSpeed += dashSpeed;
            GD.Print($"Dash activated! Direction: {_dashDirection}");
            return true;
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
            if (_isDashing)
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
                    return 1f;

                case ActiveAbilityPhase.InProgress:
                    if (_dashDuration <= 0f) return 1f;
                    return 1f - (_dashTimer / _dashDuration);

                case ActiveAbilityPhase.Cooldown:
                    if (_cooldownTime <= 0f) return 1f;
                    return 1f - (GetCooldownRemaining() / _cooldownTime);

                case ActiveAbilityPhase.Ready:
                default:
                    return 1f;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_isDashing && _owner != null)
            {
                float dashSpeed = _dashDistance / _dashDuration;
                _dashTimer -= (float)delta;

                if (_dashTimer > 0)
                {
                    _owner.Velocity = _dashDirection * dashSpeed;
                }
                else
                {
                    _owner.MaxSpeed -= dashSpeed;
                    _isDashing = false;
                    _currentPhase = ActiveAbilityPhase.Cooldown;
                    _cooldownTimer.Start();
                    GD.Print("Dash completed");
                }
            }
        }
    }
}
