using Godot;
using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace nuscutiesapp.active.characters.StateLogic
{
    public class HurtState : State, IMovementState
    {
        private const double _hurtTime = 0.5f;
        private double _totalTime = 0f;
        public override async void Enter(Character owner)
        {
            owner.AnimatedSprite.Modulate = new Color(1, 0.5f, 0.5f);

            await owner.ToSignal(owner.GetTree().CreateTimer(0.3f), SceneTreeTimer.SignalName.Timeout);

            owner.AnimatedSprite.Modulate = new Color(1, 1, 1, 1);

        }

        public override void Update(Character owner, double delta)
        {
            _totalTime += delta;


            if (_totalTime >= _hurtTime)
            {
                if (owner.Velocity.Length() >= 10)
                {
                    owner.ChangeMovementState(new MoveState());
                }
                else
                {
                    owner.ChangeMovementState(new IdleMovementState());
                }
            }
        }

        public override string ToString()
        {
            return "hurt";
        }
    }
}