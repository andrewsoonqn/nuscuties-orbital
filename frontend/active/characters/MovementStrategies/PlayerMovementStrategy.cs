using Godot;

namespace nuscutiesapp.active.characters.MovementStrategies
{
    public class PlayerMovementStrategy : IMovementStrategy
    {
        public PlayerMovementStrategy(Character character) : base(character)
        {
        }
        public override void GetDirection()
        {

            MyCharacter.MovDirection = Vector2.Zero;
            if (Input.IsActionPressed("ui_up") || Input.IsKeyPressed(Key.W))
            {
                MyCharacter.MovDirection += Vector2.Up;
            }

            if (Input.IsActionPressed("ui_down") || Input.IsKeyPressed(Key.S))
            {
                MyCharacter.MovDirection += Vector2.Down;
            }

            if (Input.IsActionPressed("ui_left") || Input.IsKeyPressed(Key.A))
            {
                MyCharacter.MovDirection += Vector2.Left;
            }

            if (Input.IsActionPressed("ui_right") || Input.IsKeyPressed(Key.D))
            {
                MyCharacter.MovDirection += Vector2.Right;
            }

        }
    }
}