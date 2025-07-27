using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public class ParticleEffectStrategy : IVisualEffectStrategy
    {
        private PackedScene _particleScene;
        private Node2D _particleInstance;

        public ParticleEffectStrategy(PackedScene particleScene)
        {
            _particleScene = particleScene;
        }

        public void ApplyEffect(Character target)
        {
            if (target != null && _particleScene != null)
            {
                _particleInstance = _particleScene.Instantiate<Node2D>();
                target.AddChild(_particleInstance);
            }
        }

        public void RemoveEffect(Character target)
        {
            if (_particleInstance != null)
            {
                _particleInstance.QueueFree();
                _particleInstance = null;
            }
        }

        public void UpdateEffect(Character target)
        {
            // Particle effects typically don't need continuous updates
            // They run autonomously once applied
        }
    }
}
