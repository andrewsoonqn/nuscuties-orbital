using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public interface IVisualEffectStrategy
    {
        void ApplyEffect(Character target);
        void RemoveEffect(Character target);
        void UpdateEffect(Character target);
    }
}
