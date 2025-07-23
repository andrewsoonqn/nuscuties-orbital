using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons.UseStrategies
{
    public partial class ProjectileUseStrategy : Node, IUseStrategy
    {
        private bool _locked = false;
        private readonly string _projectileScenePath;
        private readonly Character _wielder;
        private readonly DamageFunction _damageFunction;
        private readonly int _knockback;

        public ProjectileUseStrategy(string projectileScenePath, Character wielder, DamageFunction damageFunction, int knockback)
        {
            _projectileScenePath = projectileScenePath;
            _wielder = wielder;
            _damageFunction = damageFunction;
            _knockback = knockback;
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

            var projectileScene = GD.Load<PackedScene>(_projectileScenePath);
            var projectile = projectileScene.Instantiate<Projectile>();
            projectile.InitializeHitbox(_wielder, _damageFunction, _knockback);

            Vector2 shootingAngle = weapon.GetGlobalMousePosition() - weapon.GlobalPosition;
            shootingAngle = shootingAngle.Normalized();

            await Task.Delay(95);

            weapon.GetParent().GetParent().AddChild(projectile);
            projectile.Initialize((Marker2D)weapon.FindChild("Marker2D"), shootingAngle);
            CallDeferred(nameof(OnAttackFinished), weapon);
        }

        public async void OnAttackFinished(Weapon weapon)
        {
            await Task.Delay(weapon.GetDurationMs());
            _locked = false;
        }
    }
}