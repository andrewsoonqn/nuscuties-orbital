using Godot;
using System;

public partial class ActiveEndScene : Control
{
    [Export] private Label _message;
    [Export] private Label _rewards;
    
    private RewardManager _rewardManager;
    public override void _Ready()
    {
        _rewardManager = GetNode<RewardManager>("/root/RewardManager");
        if (_rewardManager.GameWon)
        {
            _message.Text = "You won!";
        }
        else
        {
            _message.Text = "You lost!";
        }

        _rewards.Text = $"Total EXP Gained: {_rewardManager.ExpGained}";
    }
}
