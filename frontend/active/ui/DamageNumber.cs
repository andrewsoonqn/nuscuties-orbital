using Godot;

namespace nuscutiesapp.active.ui
{
    public partial class DamageNumber : BaseNumber
    {
        protected override Color GetTextColor()
        {
            return new Color(1.0f, 0.4f, 0.2f); // Red-orange for damage
        }

        protected override string FormatText(string value)
        {
            return value; // Raw damage value
        }
    }
}