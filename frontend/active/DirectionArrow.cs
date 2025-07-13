using Godot;

namespace nuscutiesapp.active
{
    public partial class DirectionArrow : Control
    {
        [Export]
        public float ArrowDistance { get; set; } = 100.0f; // Distance from center of screen

        [Export]
        public Character Player { get; set; }

        private bool _isVisible = false;

        private EnemyTracker _enemyTracker;

        public override void _Ready()
        {
            // Initially hide the arrow
            Visible = false;

            // If ArrowTexture is not assigned, try to find it as a child
            _enemyTracker = GetNode<EnemyTracker>("/root/EnemyTracker");
        }

        public override void _Process(double delta)
        {
            if (Player == null)
                return;

            UpdateArrowDirection();
        }

        private void UpdateArrowDirection()
        {
            var nearestEnemy = _enemyTracker.GetNearestEnemy(Player.GlobalPosition);

            if (nearestEnemy == null)
            {
                HideArrow();
                return;
            }

            ShowArrow();

            // Calculate direction from player to enemy
            Vector2 directionToEnemy = (nearestEnemy.GlobalPosition - Player.GlobalPosition).Normalized();

            // Calculate arrow position on screen edge
            Vector2 arrowPosition = directionToEnemy * ArrowDistance;

            // Set arrow position
            Position = arrowPosition;

            // Rotate arrow to point toward enemy
            float angle = directionToEnemy.Angle();
            Rotation = angle;
        }

        private void ShowArrow()
        {
            if (!_isVisible)
            {
                _isVisible = true;
                Visible = true;
            }
        }

        private void HideArrow()
        {
            if (_isVisible)
            {
                _isVisible = false;
                Visible = false;
            }
        }

        public void SetPlayer(Character player)
        {
            Player = player;
        }
    }
}
