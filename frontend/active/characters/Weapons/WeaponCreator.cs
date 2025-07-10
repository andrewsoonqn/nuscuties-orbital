using Godot;
using nuscutiesapp.active.characters.Weapons.UseStrategies;
using nuscutiesapp.tools;

namespace nuscutiesapp.active.characters.Weapons
{
    public class WeaponCreator
    {
        public static Weapon CreateStaff(Character wielder)
        {
            Projectile projectile = ResourceLoader.
                Load<PackedScene>(Paths.StaffProjectile).Instantiate<Projectile>();
            return Weapon.CreateWeapon(
                Weapon.WeaponType.Staff,
                wielder,
                () => 100,
                200,
                75,
                new ProjectileUseStrategy(projectile)
            );
        }
        public static Weapon CreateSword(Character wielder)
        {
            return Weapon.CreateWeapon(
                Weapon.WeaponType.Sword,
                wielder,
                () => 100,
                200,
                250,
                new WaitForAnimationUserStrategy()
            );
        }
    }
}