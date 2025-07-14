using Godot;
using System;

namespace nuscutiesapp.active.characters.DamageSystem
{
    public partial class DamageFunction : Node
    {
        private Func<float> _damage;

        public DamageFunction(Func<float> damage) => _damage = damage;
        public DamageFunction(float damage) => _damage = () => damage;

        public float CalculateDamage()
        {
            return _damage();
        }
    }
}