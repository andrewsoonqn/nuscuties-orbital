using Godot;

namespace nuscutiesapp.active.characters.MovementStrategies
{
    public class SeekTargetMovementStrategy : IMovementStrategy
    {
        private readonly Node2D _target;
        private readonly NavigationAgent2D _agent;
        public SeekTargetMovementStrategy(Character character, Node2D target, NavigationAgent2D agent) : base(character)
        {
            this._target = target;
            this._agent = agent;
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
            MyCharacter.MovDirection = currentAgentPosition.DirectionTo(nextPathPosition);
            // Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * Speed;
            // MoveAndSlide();
        }
    }
}