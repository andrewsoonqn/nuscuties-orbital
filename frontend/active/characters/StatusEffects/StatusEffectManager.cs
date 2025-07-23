using Godot;
using System.Collections.Generic;
using System.Linq;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public partial class StatusEffectManager : Node
    {
        private Character _owner;
        private List<StatusEffect> _activeEffects = new List<StatusEffect>();

        public override void _Ready()
        {
            _owner = GetParent<Character>();
        }

        public void ApplyStatusEffect(StatusEffect effect)
        {
            if (_owner == null || effect == null) return;

            var existingEffect = _activeEffects.FirstOrDefault(e =>
                e.GetType() == effect.GetType() && e.IsActive);

            if (existingEffect != null)
            {
                existingEffect.Remove();
                _activeEffects.Remove(existingEffect);
            }

            _activeEffects.Add(effect);
            AddChild(effect);
            effect.Apply(_owner);

            effect.TreeExiting += () => OnStatusEffectRemoved(effect);
        }

        public void ApplyStatusEffect<T>() where T : StatusEffect, new()
        {
            var effect = new T();
            ApplyStatusEffect(effect);
        }

        public void RemoveStatusEffect<T>() where T : StatusEffect
        {
            var effect = _activeEffects.FirstOrDefault(e => e is T && e.IsActive);
            if (effect != null)
            {
                effect.Remove();
            }
        }

        public void RemoveAllStatusEffects()
        {
            var effectsToRemove = _activeEffects.Where(e => e.IsActive).ToList();
            foreach (var effect in effectsToRemove)
            {
                effect.Remove();
            }
        }

        public bool HasStatusEffect<T>() where T : StatusEffect
        {
            return _activeEffects.Any(e => e is T && e.IsActive);
        }

        public T GetStatusEffect<T>() where T : StatusEffect
        {
            return _activeEffects.FirstOrDefault(e => e is T && e.IsActive) as T;
        }

        public List<StatusEffect> GetActiveEffects()
        {
            return _activeEffects.Where(e => e.IsActive).ToList();
        }

        private void OnStatusEffectRemoved(StatusEffect effect)
        {
            _activeEffects.Remove(effect);
        }

    }
}