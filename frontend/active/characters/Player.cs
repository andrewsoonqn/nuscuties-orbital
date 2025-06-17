using Godot;
using System;

public partial class Player : Character
{
    private Node2D _sword;
    private AnimationPlayer _swordAnimationPlayer;
    public override void _Ready()
    {
        base._Ready();
        _sword = this.GetNode<Node2D>("Sword");
        _swordAnimationPlayer = _sword.GetNode<AnimationPlayer>("SwordAnimationPlayer");
        _sword.GetNode<Sprite2D>("SlashSprite").Visible = false;
    }

    public override void _Process(double delta)
    {
        Vector2 mouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();

        if (mouseDirection.X > 0 && AnimatedSprite.FlipH)
        // if (MovDirection.X > 0 && AnimatedSprite.FlipH)
        {
            AnimatedSprite.FlipH = false;

        }
        // else if (MovDirection.X < 0 && !AnimatedSprite.FlipH)
        else if (mouseDirection.X < 0 && !AnimatedSprite.FlipH)
        {
            AnimatedSprite.FlipH = true;
        }

        _sword.Rotation = mouseDirection.Angle();
        if (mouseDirection.X < 0 && _sword.Scale.Y > 0)
        {
            _sword.ApplyScale(new Vector2(1, -1));
        }
        else if (mouseDirection.X > 0 && _sword.Scale.Y < 0)
        {
            _sword.ApplyScale(new Vector2(1, -1));
        }

        if (Input.IsActionJustPressed("ui_attack") && !_swordAnimationPlayer.IsPlaying())
        {
            _swordAnimationPlayer.Play("attack");
        }
    }

    public override void GetInput()
    {
        MovDirection = Vector2.Zero;
        if (Input.IsActionPressed("ui_up") || Input.IsKeyPressed(Key.W))
        {
            MovDirection += Vector2.Up;
        }
        if (Input.IsActionPressed("ui_down") || Input.IsKeyPressed(Key.S))
        {
            MovDirection += Vector2.Down;
        }
        if (Input.IsActionPressed("ui_left") || Input.IsKeyPressed(Key.A))
        {
            MovDirection += Vector2.Left;
        }
        if (Input.IsActionPressed("ui_right") || Input.IsKeyPressed(Key.D))
        {
            MovDirection += Vector2.Right;
        }
    }
}