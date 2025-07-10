using Godot;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons.UseStrategies
{
    public partial class ProjectileUseStrategy : Node, IUseStrategy
    {
        private Weapon _weapon;
        private List<Projectile> _projectiles;
        private bool _locked = false;
        private Projectile _baseProjectile;

        public ProjectileUseStrategy(Projectile projectile)
        {
            _baseProjectile = projectile;
        }
        
        public void Use(Weapon weapon)
        {
            if (_locked) return;
            _weapon = weapon;
            Projectile projectile = (Projectile) _baseProjectile.Duplicate();
            _weapon.AddChild(projectile);
            _projectiles.Add(projectile);
            
            CallDeferred(MethodName.OnAttackFinished);
        }

        public async void OnAttackFinished()
        {
            await Task.Delay(_weapon.GetDurationMs());
            _locked = false;
        }
    }
}