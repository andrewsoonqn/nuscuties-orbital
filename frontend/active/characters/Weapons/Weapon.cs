using Godot;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
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
        private IUseStrategy _useStrategy;

        public override void _Ready()
        {
            Visible = true;
            
            _myHitbox.monitoring = false;

        }
        public void Use()
        {
            _useStrategy.Use(this); // TODO: explain
        }
        
        public enum WeaponType { Sword, Fist } // TODO: might change

        private static readonly Dictionary<WeaponType, string> _scenePaths =
            new()
            {
                { WeaponType.Sword, "res://active/characters/Weapons/sword.tscn" },
                { WeaponType.Fist, "res://active/characters/Weapons/fist.tscn" },
            };

        public static Weapon CreateWeapon(
            WeaponType type,
            Character wielder,
            Func<float> damageFunc,
            float knockbackMagnitude,
            int attackDurationMs,
            IUseStrategy useStrategy
        )
        {
            string weaponPath = _scenePaths.GetValueOrDefault(type);
            Weapon weapon = ResourceLoader.Load<PackedScene>(weaponPath).Instantiate<Weapon>();
            weapon._myHitbox.Initialize(wielder, damageFunc, knockbackMagnitude);
            weapon._attackDurationMs = attackDurationMs;
            weapon._useStrategy = useStrategy;
            return weapon;
        }
        
        public AnimationPlayer GetAnimationPlayer() => _animationPlayer;
        public int GetDurationMs() => _attackDurationMs;
        public Hitbox GetHitbox() => _myHitbox;
    }
}