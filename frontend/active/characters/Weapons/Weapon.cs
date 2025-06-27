using Godot;

namespace nuscutiesapp.active.characters.Weapons
{
    public abstract partial class Weapon : Node2D
    {
        private Hitbox _hitbox;
        private AnimationPlayer _animationPlayer;

        public abstract void Use();
    }
}