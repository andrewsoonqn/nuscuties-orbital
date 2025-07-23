using Godot;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.ui
{
    public partial class DamageNumberManager : Node
    {
        private PackedScene _damageNumberScene;

        public override void _Ready()
        {
            _damageNumberScene = GD.Load<PackedScene>("res://active/ui/damage_number.tscn");
        }

        public void Show(float amount, Vector2 worldPosition, Node2D world)
        {
            var damageNumber = _damageNumberScene.Instantiate<DamageNumber>();
            world.AddChild(damageNumber);
            damageNumber.Position = worldPosition;
            damageNumber.Show(amount.ToString());
        }
    }
}