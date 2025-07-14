using Godot;
using nuscutiesapp.active.characters.Weapons;
using System;
using System.Diagnostics;

public partial class WeaponSwitcher : Node
{
    Player _player;

    public override void _Ready()
    {
        _player = GetParent().GetNode<ActiveGame>("GameWorld").GetNode<Player>("Player");
        base._Ready();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey { Pressed: true } eventKey)
        {
            switch (eventKey.Keycode)
            {
                case Key.Key1:
                    SwitchWeaponTo(WeaponClass.Melee);
                    break;
                case Key.Key2:
                    SwitchWeaponTo(WeaponClass.Ranged);
                    break;
                case Key.Key3:
                    SwitchWeaponTo(WeaponClass.Utility);
                    break;
            }
        }
        base._Input(@event);
    }

    public void SwitchWeaponTo(WeaponClass wc)
    {
        _player.SwitchWeapon(wc); // TODO refactor needed here
    }
}
