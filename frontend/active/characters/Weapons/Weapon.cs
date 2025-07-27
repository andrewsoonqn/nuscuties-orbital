using Godot;
using nuscutiesapp.active.characters.DamageSystem;
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
        [Export] private int _attackDurationMs;
        public IUseStrategy UseStrategy;

        public override void _Ready()
        {
            Visible = true;

            _myHitbox.monitoring = false;

        }
        public void Use()
        {
            UseStrategy.Use(this); // TODO: explain
        }

        public enum WeaponType { Sword, Fist, Staff } // TODO: might change

        private static readonly Dictionary<WeaponType, string> _scenePaths =
            new()
            {
                { WeaponType.Sword, "res://active/characters/Weapons/sword.tscn" },
                { WeaponType.Staff, "res://active/characters/Weapons/staff.tscn" },
                { WeaponType.Fist, "res://active/characters/Weapons/fist.tscn" },
            };

        public static Weapon CreateWeapon(
            WeaponType type,
            Character wielder,
            DamageFunction damageFunc,
            float knockbackMagnitude,
            int attackDurationMs,
            IUseStrategy useStrategy
        )
        {
            string weaponPath = _scenePaths.GetValueOrDefault(type);
            Weapon weapon = ResourceLoader.Load<PackedScene>(weaponPath).Instantiate<Weapon>();
            weapon._myHitbox.Initialize(wielder, damageFunc, knockbackMagnitude);
            weapon._attackDurationMs = attackDurationMs;
            weapon.UseStrategy = useStrategy;
            return weapon;
        }

        public AnimationPlayer GetAnimationPlayer() => _animationPlayer;
        public int GetDurationMs() => _attackDurationMs;
        public Hitbox GetHitbox() => _myHitbox;

        public void Initialize(Character wielder, DamageFunction damageFunc, float knockbackMagnitude, int attackDurationMs, IUseStrategy useStrategy)
        {
            _myHitbox.Initialize(wielder, damageFunc, knockbackMagnitude);
            _attackDurationMs = attackDurationMs;
            UseStrategy = useStrategy;
        }
    }
}