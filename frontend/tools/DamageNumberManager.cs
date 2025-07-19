using Godot;

namespace nuscutiesapp.active.ui
{
    public partial class DamageNumberManager : Node
    {
        [Export] private PackedScene _damageNumberScene;

        public override void _Ready()
        {
            _damageNumberScene = GD.Load<PackedScene>("res://frontend/active/ui/damage_number.tscn");
        }

        public void Show(float amount, Vector2 worldPosition)
        {
            var damageNumber = _damageNumberScene.Instantiate<DamageNumber>();
            GetTree().Root.AddChild(damageNumber);
            damageNumber.GlobalPosition = worldPosition;
            damageNumber.Show(amount.ToString());
        }
    }
}
