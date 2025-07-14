using Godot;
using System;

public partial class WaveDisplay : Control
{
    [Export] private Label _label;
    [Export] private AnimationPlayer _animationPlayer;
    private ActiveDungeonEventManager _eventManager;


    public override void _Ready()
    {
        Hide();
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _eventManager.WaveStartEvent += Animate;
        base._Ready();
    }

    public void Animate(int wave)
    {
        _label.Text = $"Wave {wave} incoming";
        _animationPlayer.Play("show_and_fade");
    }
}