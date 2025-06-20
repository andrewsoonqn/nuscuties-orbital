using Godot;

namespace nuscutiesapp.active.characters.StateLogic
{
    public class HurtState : State, IMovementState, IActionState
    {
        private Timer _timer = new Timer();
        private const double _hurtTime = 0.5f;
        public override async void Enter(Character owner)
        {
            _timer.OneShot = true;
            _timer.Start(_hurtTime);
                
            owner.AnimatedSprite.Modulate = new Color(1, 0.5f, 0.5f);

            await owner.ToSignal(owner.GetTree().CreateTimer(0.3f), SceneTreeTimer.SignalName.Timeout);

            owner.AnimatedSprite.Modulate = new Color(1, 1, 1, 1);
        }

        public override void Update(Character owner, double delta)
        {
            if (_timer.TimeLeft == 0)
            {
                owner.ChangeActionState(new IdleState());
            }
        }
    }

}