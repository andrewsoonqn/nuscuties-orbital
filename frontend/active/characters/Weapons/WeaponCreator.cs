using Godot;
using nuscutiesapp.active.characters.DamageSystem;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.Weapons
{
    public class WeaponCreator
    {
        public static Weapon CreateStaff(Character wielder, DamageFunction damageFunction)
        {
            Projectile projectile = ResourceLoader.
                Load<PackedScene>(Paths.StaffProjectile).Instantiate<Projectile>();
            
            projectile.InitializeHitbox(wielder, damageFunction, 200);
            return Weapon.CreateWeapon(
                Weapon.WeaponType.Staff,
                wielder,
                null,
                0,
                750,
                new ProjectileUseStrategy(projectile)
            );
        }
        public static Weapon CreateSword(Character wielder, DamageFunction damageFunction)
        {
            return Weapon.CreateWeapon(
                Weapon.WeaponType.Sword,
                wielder,
                damageFunction,
                200,
                250,
                new WaitForAnimationUserStrategy()
            );
        }
    }
}