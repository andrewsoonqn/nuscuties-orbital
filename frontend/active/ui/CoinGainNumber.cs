using Godot;

namespace nuscutiesapp.active.ui
{
    public partial class CoinGainNumber : BaseNumber
    {
        protected override Color GetTextColor()
        {
            return new Color(1.0f, 0.8f, 0.2f); // Gold color for coins
        }

        protected override string FormatText(string value)
        {
            return "+" + value + " coins"; // Format as coin gain
        }
    }
}