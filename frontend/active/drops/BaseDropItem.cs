using Godot;

namespace nuscutiesapp.active.drops
{
    public abstract partial class BaseDropItem : Area2D
    {
        [Export] protected Sprite2D _sprite;
        [Export] protected CollisionShape2D _collisionShape;
        [Export] protected AnimationPlayer _animationPlayer;

        protected bool _isPickedUp = false;
        protected const float PICKUP_RANGE = 30.0f;

        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
            if (_animationPlayer != null)
            {
                _animationPlayer.Play("spawn");
            }
        }

        private void OnBodyEntered(Node2D body)
        {
            if (_isPickedUp) return;

            if (body is Player player)
            {
                OnPickup(player);
                _isPickedUp = true;

                if (_animationPlayer != null && _animationPlayer.HasAnimation("pickup"))
                {
                    _animationPlayer.Play("pickup");
                    _animationPlayer.AnimationFinished += OnPickupAnimationFinished;
                }
                else
                {
                    QueueFree();
                }
            }
        }

        protected abstract void OnPickup(Player player);

        private void OnPickupAnimationFinished(StringName animName)
        {
            if (animName == "pickup")
            {
                QueueFree();
            }
        }

        public override void _Process(double delta)
        {
            if (_isPickedUp) return;

            var player = GetTree().GetFirstNodeInGroup("player") as Player;
            if (player != null)
            {
                float distance = GlobalPosition.DistanceTo(player.GlobalPosition);
                if (distance < PICKUP_RANGE)
                {
                    Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
                    GlobalPosition += direction * 100.0f * (float)delta;
                }
            }
        }
    }
}
