using Godot;
using Godot.Collections;
using nuscutiesapp.active.characters.DamageSystem;

namespace nuscutiesapp.active.characters.ActiveAbilities
{
    public partial class Bomb : Node2D
    {
        [Export] private float _explosionRadius = 100.0f;
        [Export] private float _explosionDamage = 50.0f;
        [Export] private float _fuseTime = 2.0f;

        private Timer _fuseTimer;
        [Export] private AnimatedSprite2D _sprite;
        [Export] private AnimationPlayer _animationPlayer;
        [Export] private Area2D _explosionArea;
        [Export] private CollisionShape2D _explosionShape;

        public override void _Ready()
        {
            _fuseTimer = new Timer();
            _fuseTimer.WaitTime = _fuseTime;
            _fuseTimer.OneShot = true;
            _fuseTimer.Timeout += OnFuseTimeout;
            AddChild(_fuseTimer);

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

            // StartWarningAnimation();
        }

        // private async void StartWarningAnimation()
        // {
        //     await ToSignal(GetTree().CreateTimer(_fuseTime - 0.5f), Timer.SignalName.Timeout);
        //     if (IsInstanceValid(this) && !_fuseTimer.IsStopped())
        //     {
        //         _animationPlayer.Play("almost");
        //     }
        // }

        private void OnFuseTimeout()
        {
            StartExplosionSequence();
        }

        private async void StartExplosionSequence()
        {
            _animationPlayer.Play("almost");
            await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

            _animationPlayer.Play("explode");
            _explosionArea.Monitoring = true;

            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            var bodies = _explosionArea.GetOverlappingBodies();
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            
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
