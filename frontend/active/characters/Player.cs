using Godot;
using System;

public partial class Player : Character
{
    public override void _Process(double delta)
    {
        // Vector2 mouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();

        // if (mouseDirection.X > 0 && _animatedSprite.FlipH)
        if (_movDirection.X > 0 && _animatedSprite.FlipH)
        {
            _animatedSprite.FlipH = false;
            // } else if (mouseDirection.X < 0 && !_animatedSprite.FlipH)
        }
        else if (_movDirection.X < 0 && !_animatedSprite.FlipH)
        {
            _animatedSprite.FlipH = true;
        }
    }

    public override void GetInput()
    {
        _movDirection = Vector2.Zero;
        if (Input.IsActionPressed("ui_up") || Input.IsKeyPressed(Key.W))
        {
            _movDirection += Vector2.Up;
        }
        else if (Input.IsActionPressed("ui_down") || Input.IsKeyPressed(Key.S))
        {
            _movDirection += Vector2.Down;
        }
        else if (Input.IsActionPressed("ui_left") || Input.IsKeyPressed(Key.A))
        {
            _movDirection += Vector2.Left;
        }
        else if (Input.IsActionPressed("ui_right") || Input.IsKeyPressed(Key.D))
        {
            _movDirection += Vector2.Right;
        }
    }
}