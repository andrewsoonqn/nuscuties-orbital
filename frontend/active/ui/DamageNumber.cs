using Godot;

namespace nuscutiesapp.active.ui
{
    [ScriptPath("res://frontend/active/ui/DamageNumber.cs")]
    public partial class DamageNumber : Label
    {
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public void Show(string text)
        {
            Text = text;
            _animationPlayer.Play("show");
        }
    }
}
