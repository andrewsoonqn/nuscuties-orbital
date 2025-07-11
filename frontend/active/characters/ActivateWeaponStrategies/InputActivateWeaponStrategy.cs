using Godot;
using nuscutiesapp.active.characters.Weapons;

namespace nuscutiesapp.active.characters.ActivateWeaponStrategies
{
    public partial class InputActivateWeaponStrategy : Node, IActivateWeaponStrategy
    {
        public void Activate(Weapon weapon, Character attacker)
        {
            if (Input.IsActionJustPressed("ui_attack"))
            {
                weapon.Use();
            }
        }
    }
}