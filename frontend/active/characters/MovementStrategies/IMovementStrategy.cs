using Godot;

namespace nuscutiesapp.active.characters.MovementStrategies
{
    public abstract class IMovementStrategy
    {
        protected Character MyCharacter;

        protected IMovementStrategy(Character character)
        {
            MyCharacter = character;
        }

        public abstract void GetDirection();
    }
}