using Godot;

namespace nuscutiesapp.active.ui
{
    public abstract partial class BaseNumber : Node2D
    {
        [Export] private AnimationPlayer _animationPlayer;
        [Export] private Label _label;

        protected abstract Color GetTextColor();
        protected abstract string FormatText(string value);

        public virtual void Show(string text)
        {
            _label.Text = FormatText(text);
            _label.Modulate = GetTextColor();
            _animationPlayer.Play("show");
        }
    }
}