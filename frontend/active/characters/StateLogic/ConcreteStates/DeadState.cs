using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    public class DeadState : State, IActionState
    {
        public override async void Enter(Character owner)
        {
            // If we had a death animation, we would play it here.
            await owner.PlayDeathAnimation();
        
            // At this point, the character is invisible. You might want to remove them
            // from the scene entirely to free up resources.
            owner.QueueFree(); 
        }

        public override string ToString()
        {
            return "dead";
        }

        public override bool IsAllLayerState()
        {
            return true;
        }
    }
}
