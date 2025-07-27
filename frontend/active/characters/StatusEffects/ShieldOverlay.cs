using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class ShieldOverlay : Sprite2D
    {
        [Export] public float DefaultTransparency = 0.7f;
        [Export] public Vector2 DefaultScale = new Vector2(1.5f, 1.5f);

        public override void _Ready()
        {
            Modulate = new Color(1, 1, 1, DefaultTransparency);
            Scale = DefaultScale;
        }
    }
}
