using Godot;
using nuscutiesapp.active.characters.Weapons;

namespace nuscutiesapp.active.characters.ActivateWeaponStrategies
{
    public partial class InputActivateWeaponStrategy : Node, IActivateWeaponStrategy
    {
        public void Activate(Weapon weapon, Character attacker)
        {
            GD.Print("activated input");
            if (Input.IsActionJustPressed("ui_attack"))
            {
                GD.Print("atacke press");
                weapon.Use();
            }
        }
    }
}