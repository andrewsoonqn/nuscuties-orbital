using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public class ModulationStrategy : IVisualEffectStrategy
    {
        private Color _modulationColor;
        private bool _isApplied = false;

        public ModulationStrategy(Color modulationColor)
        {
            _modulationColor = modulationColor;
        }

        public void ApplyEffect(Character target)
        {
            if (target?.MyColorManager != null)
            {
                target.MyColorManager.SetBaseColor(_modulationColor);
                _isApplied = true;
            }
        }

        public void RemoveEffect(Character target)
        {
            if (target?.MyColorManager != null)
            {
                target.MyColorManager.ResetToOriginal();
                _isApplied = false;
            }
        }

        public void UpdateEffect(Character target)
        {
        }
    }
}
