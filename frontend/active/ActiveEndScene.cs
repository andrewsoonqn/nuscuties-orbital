using Godot;
using nuscutiesapp.tools;
using System;

public partial class ActiveEndScene : Control
{
    [Export] private Label _message;
    [Export] private Label _rewards;
    [Export] private Button _backButton;

    private RewardManager _rewardManager;
    private ExpManager _expManager;
    public override void _Ready()
    {
        _expManager = GetNode<ExpManager>("/root/ExpManager");
        _rewardManager = GetNode<RewardManager>("/root/RewardManager");
        _expManager.AddExp(_rewardManager.ExpGained);
        // TODO this logic should be in reward manager, ^^
        // but to reduce amount of updates, it is temporarily placed here
        if (_rewardManager.GameWon)
        {
            _message.Text = "You won!";
        }
        else
        {
            _message.Text = "You lost!";
        }

        _rewards.Text = $"Total EXP Gained: {_rewardManager.ExpGained}";

        _backButton.Pressed += BackButtonOnPressed;
    }

    private void BackButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.Active);
    }
}