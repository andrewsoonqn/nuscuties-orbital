using nuscutiesapp.active.characters.Weapons;

namespace nuscutiesapp.active.characters.ActivateWeaponStrategies
{
    public class AlwaysActivateWeaponStrategy : IActivateWeaponStrategy
    {
        public void Activate(Weapon weapon, Character attacker)
        {
            weapon.Use();
        }
    }
}