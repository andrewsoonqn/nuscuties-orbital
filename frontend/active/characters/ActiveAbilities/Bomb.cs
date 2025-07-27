using Godot;
using nuscutiesapp.active.characters.DamageSystem;

namespace nuscutiesapp.active.characters.ActiveAbilities
{
    public partial class Bomb : Node2D
    {
        [Export] private float _explosionRadius = 100.0f;
        [Export] private float _explosionDamage = 50.0f;
        [Export] private float _fuseTime = 2.0f;

        private Timer _fuseTimer;
        private Sprite2D _sprite;
        private AnimationPlayer _animationPlayer;
        private Area2D _explosionArea;
        private CollisionShape2D _explosionShape;

        public override void _Ready()
        {
            _fuseTimer = new Timer();
            _fuseTimer.WaitTime = _fuseTime;
            _fuseTimer.OneShot = true;
            _fuseTimer.Timeout += OnFuseTimeout;
            AddChild(_fuseTimer);

            _sprite = GetNode<Sprite2D>("Sprite2D");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            _explosionArea = GetNode<Area2D>("ExplosionArea");
            _explosionShape = GetNode<CollisionShape2D>("ExplosionArea/CollisionShape2D");

            var circleShape = new CircleShape2D();
            circleShape.Radius = _explosionRadius;
            _explosionShape.Shape = circleShape;

            _explosionArea.Monitoring = false;
            _explosionArea.Monitorable = false;

            _animationPlayer.Play("idle");
        }

        public void Initialize(Vector2 position, float explosionRadius, float explosionDamage, float fuseTime)
        {
            GlobalPosition = position;
            _explosionRadius = explosionRadius;
            _explosionDamage = explosionDamage;
            _fuseTime = fuseTime;
            _fuseTimer.WaitTime = _fuseTime;
            _fuseTimer.Start();

            var circleShape = new CircleShape2D();
            circleShape.Radius = _explosionRadius;
            _explosionShape.Shape = circleShape;
        }

        private void OnFuseTimeout()
        {
            Explode();
        }

        private async void Explode()
        {
            _animationPlayer.Play("explode");
            _explosionArea.Monitoring = true;

            var bodies = _explosionArea.GetOverlappingBodies();
            foreach (var body in bodies)
            {
                if (body is Character character)
                {
                    var damageInfo = new DamageInfo(
                        _explosionDamage,
                        Vector2.Right
                    );
                    character.TakeDamage(damageInfo);
                }
            }

            await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
            QueueFree();
        }
    }
}
