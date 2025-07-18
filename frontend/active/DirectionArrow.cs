using Godot;
using System.Collections.Generic;

namespace nuscutiesapp.active
{
    public partial class DirectionArrow : Control
    {
        [Export]
        public float ArrowDistance { get; set; } = 100.0f; // Distance from center of screen

        public Character Player { get; set; }

        private bool _isVisible = false;

        private EnemyTracker _enemyTracker;

        private ActiveDungeonEventManager _eventManager;

        private HashSet<Node2D> _deadEnemies = new HashSet<Node2D>();

        public override void _Ready()
        {
            Player = (Character)GetParent();
            // Initially hide the arrow
            Visible = false;

            // If ArrowTexture is not assigned, try to find it as a child
            _enemyTracker = GetNode<EnemyTracker>("/root/EnemyTracker");

            _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
            _eventManager.EnemyDiedEvent += OnEnemyDied;
        }

        public override void _ExitTree()
        {
            // Unsubscribe from events to prevent callbacks after object disposal
            if (_eventManager != null)
            {
                _eventManager.EnemyDiedEvent -= OnEnemyDied;
            }
            base._ExitTree();
        }

        private void OnEnemyDied()
        {
            // Check if Player is still valid before accessing
            if (Player == null || !IsInstanceValid(Player))
            {
                return;
            }

            var nearestEnemy = _enemyTracker?.GetNearestEnemy(Player.GlobalPosition);
            if (nearestEnemy != null)
            {
                _deadEnemies.Add(nearestEnemy);
            }

            HideArrow();
            UpdateArrowDirection();
        }

        public override void _Process(double delta)
        {
            if (Player == null || !IsInstanceValid(Player))
                return;

            UpdateArrowDirection();
        }

        private void UpdateArrowDirection()
        {
            if (Player == null || !IsInstanceValid(Player))
                return;

            var nearestEnemy = _enemyTracker?.GetNearestEnemy(Player.GlobalPosition);

            if (nearestEnemy == null || _deadEnemies.Contains(nearestEnemy))
            {
                HideArrow();
                return;
            }

            // Calculate direction from player to enemy
            Vector2 directionToEnemy = (nearestEnemy.GlobalPosition - Player.GlobalPosition).Normalized();

            // Calculate arrow position on screen edge
            Vector2 arrowPosition = directionToEnemy * ArrowDistance;

            // Set arrow position
            Position = arrowPosition;

            // Rotate arrow to point toward enemy
            float angle = directionToEnemy.Angle();
            Rotation = angle;
            ShowArrow();
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
