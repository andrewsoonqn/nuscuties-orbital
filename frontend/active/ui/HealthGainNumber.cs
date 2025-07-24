using Godot;

namespace nuscutiesapp.active.ui
{
    public partial class HealthGainNumber : BaseNumber
    {
        protected override Color GetTextColor()
        {
            return new Color(0.2f, 1.0f, 0.3f); // Bright green for health gain
        }

        protected override string FormatText(string value)
        {
            return "+" + value + " HP"; // Add plus sign for positive health
        }
    }
}
