using nuscutiesapp.active.characters.Weapons;

namespace nuscutiesapp.active.characters.ActivateWeaponStrategies
{
    public interface IActivateWeaponStrategy
    {
        public void Activate(Weapon weapon, Character attacker);
    }
}