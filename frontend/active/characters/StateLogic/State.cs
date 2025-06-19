using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    /// <summary>
    /// Convenience base class that provides empty virtual methods for any state implementation.
    /// </summary>
    public abstract class State : IState
    {
        /// <summary>
        /// Called when the state is entered. This is a virtual method with an empty implementation, to be overridden by derived classes.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        public virtual void Enter(Character owner) { }

        /// <summary>
        /// Called when the state is exited. This is a virtual method with an empty implementation, to be overridden by derived classes.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        public virtual void Exit(Character owner) { }

        /// <summary>
        /// Called every frame while the state is active. This is a virtual method with an empty implementation, to be overridden by derived classes.
        /// </summary>
        /// <param name="owner">The character that owns this state machine.</param>
        /// <param name="delta">The time elapsed since the last frame.</param>
        public virtual void Update(Character owner, double delta) { }

        /// <summary>
        /// Determines if this state affects all layers (movement and action). By default, a state only affects one layer.
        /// Override this method if the state spans both layers.
        /// </summary>
        /// <returns>True if the state is an all-layer state, false otherwise.</returns>
        public virtual bool IsAllLayerState() => false;
    }
}