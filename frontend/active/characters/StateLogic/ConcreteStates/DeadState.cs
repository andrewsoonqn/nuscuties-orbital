using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    public class DeadState : State, IActionState
    {
        public override async void Enter(Character owner)
        {
            await owner.PlayDeathAnimation();

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