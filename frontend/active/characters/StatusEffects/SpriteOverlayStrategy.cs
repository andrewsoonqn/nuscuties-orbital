using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public class SpriteOverlayStrategy : IVisualEffectStrategy
    {
        private PackedScene _overlayScene;
        private Node2D _overlayInstance;

        public SpriteOverlayStrategy(PackedScene overlayScene)
        {
            _overlayScene = overlayScene;
        }

        public void ApplyEffect(Character target)
        {
            if (target != null && _overlayScene != null)
            {
                _overlayInstance = _overlayScene.Instantiate<Node2D>();

                target.AddChild(_overlayInstance);
            }
        }

        public void RemoveEffect(Character target)
        {
            if (_overlayInstance != null)
            {
                _overlayInstance.QueueFree();
                _overlayInstance = null;
            }
        }

        public void UpdateEffect(Character target)
        {
            if (_overlayInstance != null)
            {
                _overlayInstance.Position = Vector2.Zero;
            }
        }
    }
}