using Godot;
using System;
using System.Threading.Tasks;

namespace nuscutiesapp.active.characters.Weapons.UseStrategies
{
    public partial class WaitForAnimationUserStrategy : Node, IUseStrategy
    {
        private Weapon _weapon;
        public void Use(Weapon weapon)
        {
            AnimationPlayer animationPlayer = weapon.GetAnimationPlayer();
            if (animationPlayer == null)
            {
                throw new ArgumentException("Weapon does not have an animation player.");
            }

            _weapon = weapon;

            animationPlayer.AnimationStarted += OnAnimationStarted;

            if (!animationPlayer.IsPlaying())
            {
                animationPlayer.Play("attack");
            }
        }
        private async void OnAttackFinished(StringName animName)
        {
            await Task.Delay(_weapon.GetDurationMs());
            _weapon.GetHitbox().monitoring = false;
        }

        private void OnAnimationStarted(StringName animName)
        {
            CallDeferred(MethodName.OnAttackFinished, animName);
            _weapon.GetHitbox().monitoring = true;
        }
    }
}