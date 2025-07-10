using Godot;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons
{
    public partial class Projectile : Node2D
    {
        [Export] private Hitbox _projectileHitbox;
        [Export] private Timer _timer;
        [Export] private int _lifetimeMs = 5000;

        private Vector2 _direction;
        private readonly int _speed = 10;

        public override void _Ready()
        {
            _timer.OneShot = true;
            _timer.Start((float)_lifetimeMs/1000);
            
            _timer.Timeout += TimerOnTimeout;
            base._Ready();
        }

        public void Initialize(Marker2D marker, Vector2 dir)
        {
            Position = marker.GlobalPosition;
            _direction = dir;
            GlobalRotation = dir.Angle();
        }

        public override void _PhysicsProcess(double delta)
        {
            GlobalPosition += _direction * _speed * (float) delta;
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