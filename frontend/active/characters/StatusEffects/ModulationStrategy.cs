using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public class ModulationStrategy : IVisualEffectStrategy
    {
        private Color _modulationColor;
        private Color _originalColor;
        private bool _isApplied = false;

        public ModulationStrategy(Color modulationColor)
        {
            _modulationColor = modulationColor;
            _originalColor = new Color(1.0f, 1.0f, 1.0f);
        }

        public void ApplyEffect(Character target)
        {
            if (target?.AnimatedSprite != null)
            {
                _originalColor = target.AnimatedSprite.Modulate;
                target.AnimatedSprite.Modulate = _modulationColor;
                _isApplied = true;
            }
        }

        public void RemoveEffect(Character target)
        {
            if (target?.AnimatedSprite != null)
            {
                target.AnimatedSprite.Modulate = _originalColor;
                _isApplied = false;
            }
        }

        public void UpdateEffect(Character target)
        {
        }
    }
}
