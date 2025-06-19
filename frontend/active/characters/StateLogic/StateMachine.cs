using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    /// <summary>
    /// Manages the states for a character, transitioning between them and updating the current state.
    /// </summary>
    /// <typeparam name="T">The type of state, which must implement <see cref="IState"/>.</typeparam>
    public class StateMachine<T> where T : IState
    {
        /// <summary>
        /// Gets the current active state of the state machine.
        /// </summary>
        public T CurrentState { get; private set; }
        /// <summary>
        /// Gets the character that owns this state machine.
        /// </summary>
        public Character Owner { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        /// <param name="initialState">The initial state to set when the state machine is created.</param>
        public StateMachine(Character owner, T initialState)
        {
            Owner = owner;
            SetState(initialState);
        }

        /// <summary>
        /// Sets the current state of the state machine.
        /// Exits the current state and enters the new state.
        /// </summary>
        /// <param name="newState">The new state to transition to.</param>
        public void SetState(T newState)
        {
            // Ignore if identical.
            if (Equals(CurrentState, newState)) return;

            CurrentState?.Exit(Owner);
            CurrentState = newState;
            CurrentState.Enter(Owner);
        }

        /// <summary>
        /// Updates the current state every frame.
        /// </summary>
        /// <param name="delta">The time elapsed since the last frame.</param>
        public void Update(double delta)
        {
            CurrentState?.Update(Owner, delta);
        }

        /// <summary>
        /// Determines if the current state is an all-layer state.
        /// </summary>
        /// <returns>True if the current state is an all-layer state, false otherwise.</returns>
        private bool IsAllLayerState()
        {
            return CurrentState.IsAllLayerState();
        }
    }
}