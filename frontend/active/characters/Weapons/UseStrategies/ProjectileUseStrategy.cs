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
        private bool _locked = false;
        private Projectile _baseProjectile;

        public ProjectileUseStrategy(Projectile projectile)
        {
            _baseProjectile = projectile;
        }
        
        public async void Use(Weapon weapon)
        {
            AnimationPlayer animationPlayer = weapon.GetAnimationPlayer();
            if (animationPlayer == null)
            {
                throw new ArgumentException("Weapon does not have an animation player.");
            }
            
            if (!animationPlayer.IsPlaying())
            {
                animationPlayer.Play("attack");
            }
            
            if (_locked) return;
            _locked = true;
            _weapon = weapon;
            Projectile projectile = (Projectile) _baseProjectile.Duplicate();
            Vector2 shootingAngle = weapon.GetGlobalMousePosition() - weapon.GlobalPosition;
            shootingAngle = shootingAngle.Normalized();
            
            await Task.Delay(95);
            
            projectile.Initialize((Marker2D)weapon.FindChild("Marker2D"), shootingAngle);

            _weapon.GetParent().GetParent().AddChild(projectile);
            CallDeferred(MethodName.OnAttackFinished);
        }

        public async void OnAttackFinished()
        {
            await Task.Delay(_weapon.GetDurationMs());
            _locked = false;
        }
    }
}