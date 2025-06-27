using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    /// <summary>
    /// Default idle state. Used for both movement and actions when nothing is occurring.
    /// </summary>
    public class IdleActionState : State, IActionState
    {
        /// <summary>
        /// Called when the idle state is entered. Plays the "idle" animation.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        public override void Enter(Character owner)
        {
        }

        /// <summary>
        /// Called every frame while the idle state is active. No specific actions are performed here as it's an idle state.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        /// <param name="delta">The time elapsed since the last frame.</param>
        public override void Update(Character owner, double delta)
        {
        }
        public override string ToString()
        {
            return "idle";
        }
    }
}