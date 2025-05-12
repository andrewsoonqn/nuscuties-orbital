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
        } else if (_movDirection.X < 0 && !_animatedSprite.FlipH)
        {
            _animatedSprite.FlipH = true;
        }
    }
    
    public override void GetInput()
    {
        _movDirection = Vector2.Zero;
        if (Input.IsActionPressed("ui_up"))
        {
            GD.Print("up");
            _movDirection += Vector2.Up;
        } else if (Input.IsActionPressed("ui_down"))
        {
            _movDirection += Vector2.Down;
        } else if (Input.IsActionPressed("ui_left"))
        {
            _movDirection += Vector2.Left;
        } else if (Input.IsActionPressed("ui_right"))
        {
            _movDirection += Vector2.Right;
        }
    }
}