using Godot;
using System.Collections.Generic;
using System.Linq;

namespace nuscutiesapp.active
{
    public partial class EnemyTracker : Node
    {
        private List<Node2D> _activeEnemies = new List<Node2D>();

        public void RegisterEnemy(Node2D enemy)
        {
            if (!_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Add(enemy);

                // Connect to enemy's tree_exited signal to remove it when destroyed
                enemy.TreeExited += () => UnregisterEnemy(enemy);
            }
        }

        public void UnregisterEnemy(Node2D enemy)
        {
            _activeEnemies.Remove(enemy);
        }

        public Node2D GetNearestEnemy(Vector2 playerPosition)
        {
            if (_activeEnemies.Count == 0)
                return null;

            // Remove any null/freed enemies
            _activeEnemies.RemoveAll(enemy => enemy == null || !IsInstanceValid(enemy));

            if (_activeEnemies.Count == 0)
                return null;

            return _activeEnemies
                .OrderBy(enemy => playerPosition.DistanceSquaredTo(enemy.GlobalPosition))
                .FirstOrDefault();
        }

        public int GetEnemyCount()
        {
            // Clean up null/freed enemies before returning count
            _activeEnemies.RemoveAll(enemy => enemy == null || !IsInstanceValid(enemy));
            return _activeEnemies.Count;
        }

        public List<Node2D> GetAllEnemies()
        {
            // Clean up null/freed enemies
            _activeEnemies.RemoveAll(enemy => enemy == null || !IsInstanceValid(enemy));
            return new List<Node2D>(_activeEnemies);
        }
    }
}