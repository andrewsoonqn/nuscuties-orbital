using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using System;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons
{
    public partial class Projectile : Node2D
    {
        [Export] private Hitbox _projectileHitbox;
        [Export] private Timer _timer;
        [Export] private int _lifetimeMs = 5000;

        private Vector2 _direction;
        private readonly int _speed = 100;

        public override void _Ready()
        {
            if (_timer == null) _timer = (Timer)FindChild("Timer");
            _timer.OneShot = true;
            _timer.Start((float)_lifetimeMs / 1000);

            _timer.Timeout += TimerOnTimeout;

            _projectileHitbox.BodyEntered += ProjectileHitboxOnBodyEntered;

            base._Ready();
        }

        private void ProjectileHitboxOnBodyEntered(Node2D body)
        {
            if (body.IsInGroup("projectile_collision"))
            {
                QueueFree();
            }
        }

        public void Initialize(Marker2D marker, Vector2 dir)
        {
            GlobalPosition = marker.GlobalPosition;
            _direction = dir;
            GlobalRotation = dir.Angle();
        }

        public void InitializeHitbox(Character wielder, DamageFunction damageFunc, int knockbackMagnitude, string statusEffect = null)
        {
            if (_projectileHitbox == null)
            {
                _projectileHitbox = (Hitbox)FindChild("Hitbox");
            }
            _projectileHitbox.Initialize(wielder, damageFunc, knockbackMagnitude, statusEffect);
        }
        public override void _PhysicsProcess(double delta)
        {
            GlobalPosition += _direction * _speed * (float)delta;
            base._PhysicsProcess(delta);
        }

        private void TimerOnTimeout()
        {
            QueueFree();
        }

        public void Activate()
        {
            _projectileHitbox.monitoring = true;
        }

        public void Deactivate()
        {
            _projectileHitbox.monitoring = false;
        }
    }
}