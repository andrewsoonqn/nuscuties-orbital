using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    /// <summary>
    /// Defines the interface for a state in a state machine.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        void Enter(Character owner);

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        void Exit(Character owner);

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        /// <param name="delta">The time elapsed since the last frame.</param>
        void Update(Character owner, double delta);

        /// <summary>
        /// Determines if the state is an all-layer state.
        /// </summary>
        /// <returns>True if the state is an all-layer state, false otherwise.</returns>
        bool IsAllLayerState() => false;
    }
}