using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons
{
    public partial class Weapon : Node2D
    {
        [Export] private Hitbox _myHitbox;
        [Export] private AnimationPlayer _animationPlayer;
        private int _attackDurationMs;

        public override void _Ready()
        {
            Visible = true;
            
            _myHitbox.monitoring = false;
            
            _animationPlayer.AnimationStarted += OnAnimationStarted;
        }
        public void Use()
        {
            if (!_animationPlayer.IsPlaying())
            {
                _animationPlayer.Play("attack");
            }
        }
        
        private async void OnAttackFinished(StringName animName)
        {
            await Task.Delay(_attackDurationMs);
            _myHitbox.monitoring = false;
        }

        private void OnAnimationStarted(StringName animName)
        {
            CallDeferred(MethodName.OnAttackFinished, animName);
            _myHitbox.monitoring = true;
        }
        public enum WeaponType { Sword, Spear } // TODO: might change

        private static readonly Dictionary<WeaponType, string> _scenePaths =
            new()
            {
                { WeaponType.Sword, "res://active/characters/Weapons/sword.tscn" },
                { WeaponType.Spear, "res://active/characters/Weapons/spear.tscn" },
            };

        public static Weapon CreateWeapon(
            WeaponType type,
            Character wielder,
            Func<float> damageFunc,
            float knockbackMagnitude,
            int attackDurationMs 
        )
        {
            string weaponPath = _scenePaths.GetValueOrDefault(type);
            Weapon weapon = ResourceLoader.Load<PackedScene>(weaponPath).Instantiate<Weapon>();
            weapon._myHitbox.Initialize(wielder, damageFunc, knockbackMagnitude);
            weapon._attackDurationMs = attackDurationMs;
            return weapon;
        }
    }
}