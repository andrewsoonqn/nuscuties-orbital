using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    /// <summary>
    /// Handles movement logic for the character when walking/running around.
    /// </summary>
    public class MoveState : State, IMovementState
    {
        public override void Enter(Character owner)
        {
            owner.PlayMoveAnimation();
        }

        public override void Update(Character owner, double delta)
        {
            if (owner == null)
            {
                return;
            }

            owner.GetDirection();

            owner.Move();

            if (owner.Velocity.Length() < 10)
            {
                owner.ChangeMovementState(new IdleState());
            }
        }
    }
}