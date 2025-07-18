using Godot;
using System;

public partial class WaveDisplay : Control
{
    private Label _label;
    private AnimationPlayer _animationPlayer;
    private ActiveDungeonEventManager _eventManager;


    public override void _Ready()
    {
        _label = (Label)FindChild("Label");
        _animationPlayer = (AnimationPlayer)FindChild("AnimationPlayer");
        Hide();
        _eventManager = GetNode<ActiveDungeonEventManager>("/root/ActiveDungeonEventManager");
        _eventManager.WaveStartEvent += Animate;
        base._Ready();
    }

    public override void _ExitTree()
    {
        if (_eventManager != null)
        {
            _eventManager.WaveStartEvent -= Animate;
        }
        base._ExitTree();
    }

    public void Animate(int wave)
    {
        _label.Text = $"Wave {wave} incoming";
        _animationPlayer.Play("show_and_fade");
    }


}