using Godot;

namespace nuscutiesapp.active.characters.MovementStrategies
{
    public class SeekTargetMovementStrategy : IMovementStrategy
    {
        private readonly Node2D _target;
        private readonly NavigationAgent2D _agent;
        private Vector2 _safeVelocity;
        public SeekTargetMovementStrategy(Character character, Node2D target, NavigationAgent2D agent) : base(character)
        {
            this._target = target;
            this._agent = agent;
            
            _agent.VelocityComputed += AgentOnVelocityComputed;
            _safeVelocity = Vector2.Zero;
        }

        private void AgentOnVelocityComputed(Vector2 safeVelocity)
        {
            _safeVelocity = safeVelocity.Normalized();
        }

        public override void GetDirection()
        {
            MyCharacter.MovDirection = Vector2.Zero;
            if (_target != null)
            {
                _agent.TargetPosition = _target.GlobalPosition;
            }
            if (_agent.IsNavigationFinished()) return;

            Vector2 currentAgentPosition = MyCharacter.GlobalPosition;
            Vector2 nextPathPosition = _agent.GetNextPathPosition();

            if (_safeVelocity != Vector2.Zero)
            {
                MyCharacter.MovDirection = _safeVelocity;
                _safeVelocity = Vector2.Zero;
                return;
            }
            MyCharacter.MovDirection = currentAgentPosition.DirectionTo(nextPathPosition);
            // Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * Speed;
            // MoveAndSlide();
        }
    }
}