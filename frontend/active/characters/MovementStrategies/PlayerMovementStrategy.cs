using Godot;
using nuscutiesapp.active.characters.ActiveAbilities;

namespace nuscutiesapp.active.characters.MovementStrategies
{
    public class PlayerMovementStrategy : IMovementStrategy
    {
        public PlayerMovementStrategy(Character character) : base(character)
        {
        }

        public override void GetDirection()
        {
            if (IsPlayerDashing())
            {
                MyCharacter.MovDirection = Vector2.Zero;
                return;
            }

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

        private bool IsPlayerDashing()
        {
            if (MyCharacter is Player player)
            {
                var loadout = player.GetCurrentLoadout();
                if (loadout?.ActiveAbilities != null)
                {
                    foreach (var ability in loadout.ActiveAbilities)
                    {
                        if (ability is DashAbility dashAbility && dashAbility.IsDashing)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
