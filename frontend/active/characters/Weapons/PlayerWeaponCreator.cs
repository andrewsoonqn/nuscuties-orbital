using nuscutiesapp.active.characters.Weapons.UseStrategies;

namespace nuscutiesapp.active.characters.Weapons
{
    public class WeaponCreator
    {
        public static Weapon CreateStaff(Character wielder)
        {
            return Weapon.CreateWeapon(
                Weapon.WeaponType.Staff,
                wielder,
                () => 100,
                200,
                75,
                new WaitForAnimationUserStrategy()
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