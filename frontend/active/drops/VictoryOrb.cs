using Godot;

namespace nuscutiesapp.active.drops
{
    public partial class VictoryOrb : BaseDropItem
    {
        private ActiveDungeonEventManager _eventManager;

        public override void _Ready()
        {
            _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
            _animationPlayer.Play("idle");
            base._Ready();
        }

        protected override void OnPickup(Player player)
        {
            GD.Print("Victory Orb collected! Game completed!");
            _eventManager.GameWon();
        }
    }
}