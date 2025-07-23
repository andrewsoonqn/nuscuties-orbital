using Godot;

namespace nuscutiesapp.active.ui
{
    public partial class DamageNumber : Node2D
    {
        [Export] private AnimationPlayer _animationPlayer;
        [Export] private Label _label;

        public void Show(string text)
        {
            _label.Text = text;
            _animationPlayer.Play("show");
        }
    }
}