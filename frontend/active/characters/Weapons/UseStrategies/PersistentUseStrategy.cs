using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons.UseStrategies
{
    public partial class NoAnimationUseStrategy : Node, IUseStrategy
    {
        private Weapon _weapon;
        public void Use(Weapon weapon)
        {
            _weapon = weapon;
            _weapon.GetHitbox().monitoring = true;
            // CallDeferred(MethodName.OnAttackFinished);
        }

        public async void OnAttackFinished()
        {
            // await Task.Delay(_weapon.GetDurationMs());
            // _weapon.GetHitbox().monitoring = false;
        }
    }
}