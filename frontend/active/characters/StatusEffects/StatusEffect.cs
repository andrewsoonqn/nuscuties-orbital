using Godot;

namespace nuscutiesapp.active.characters.StatusEffects
{
    public abstract partial class StatusEffect : Node
    {
        [Export] protected float _duration = 3.0f;
        [Export] protected float _tickInterval = 1.0f;

        protected Character _target;
        protected float _remainingDuration;
        protected float _timeSinceLastTick;
        protected bool _isActive = false;

        public float RemainingDuration => _remainingDuration;
        public bool IsActive => _isActive;
        public abstract string StatusName { get; }

        public override void _Ready()
        {
            _remainingDuration = _duration;
            _timeSinceLastTick = 0.0f;
        }

        public void Apply(Character target)
        {
            _target = target;
            _isActive = true;
            _remainingDuration = _duration;
            _timeSinceLastTick = 0.0f;

            OnApplied();
            GD.Print($"Applied {StatusName} to {target.Name} for {_duration} seconds");
        }

        public void Remove()
        {
            if (!_isActive) return;

            _isActive = false;
            OnRemoved();
            GD.Print($"Removed {StatusName} from {_target?.Name}");

            QueueFree();
        }

        public override void _Process(double delta)
        {
            if (!_isActive || _target == null) return;

            _remainingDuration -= (float)delta;
            _timeSinceLastTick += (float)delta;

            if (_timeSinceLastTick >= _tickInterval)
            {
                OnTick();
                _timeSinceLastTick = 0.0f;
            }

            if (_remainingDuration <= 0)
            {
                Remove();
            }
        }

        protected virtual void OnApplied() { }
        protected virtual void OnRemoved() { }
        protected virtual void OnTick() { }
    }
}
