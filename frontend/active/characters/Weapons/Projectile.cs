using Godot;

namespace nuscutiesapp.active.characters.Weapons
{
    public partial class Projectile : Node2D
    {
        [Export] private Hitbox _projectileHitbox;
        [Export] private Timer _timer;
        [Export] private int _lifetimeMs = 500;

        public override void _Ready()
        {
            _timer.OneShot = true;
            _timer.Start(_lifetimeMs);
            
            _timer.Timeout += TimerOnTimeout;
            base._Ready();
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