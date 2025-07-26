using Godot;
using nuscutiesapp.tools;

public partial class PassiveOngoing : Control
{
    [Export] Label _timeSpentLabel;
    [Export] Label _totalTimeLabel;
    [Export] ProgressBar _timeProgressBar;
    [Export] Label _expAccumulatedLabel;
    [Export] Label _coinsAccumulatedLabel;
    [Export] Button _quitButton;

    private double _totalTime;
    private double _timeSpent = 0;
    private int _expAccumulated = 0;
    private int _coinsAccumulated = 0;
    private PassiveSessionInfoManager _passiveSessionInfoManager;
    private PlayerInventoryManager _inventoryManager;

    public override void _Ready()
    {
        _passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");
        _inventoryManager = this.GetNode<PlayerInventoryManager>("/root/PlayerInventoryManager");
        this._totalTime = _passiveSessionInfoManager.getTotalTime();
        _timeSpentLabel.Text = _timeSpent.ToString("F0");
        _totalTimeLabel.Text = $"out of {_totalTime:F0} seconds";
        _expAccumulatedLabel.Text = $"{_expAccumulated:F0} EXP";
        if (_coinsAccumulatedLabel != null)
        {
            _coinsAccumulatedLabel.Text = $"{_coinsAccumulated:F0} Coins";
        }

        _quitButton.Pressed += () => OnEnd("Quit");
    }

    public override void _Process(double delta)
    {
        _timeSpent += delta;
        _timeSpentLabel.Text = _timeSpent.ToString("F0");
        _timeProgressBar.Ratio = _timeSpent / _totalTime;

        _expAccumulated = (int)_timeSpent / 2 * 100;
        _expAccumulatedLabel.Text = $"{_expAccumulated:F0} EXP";

        _coinsAccumulated = _inventoryManager.CalculatePassiveDungeonCoinReward(_timeSpent / 60.0);
        if (_coinsAccumulatedLabel != null)
        {
            _coinsAccumulatedLabel.Text = $"{_coinsAccumulated:F0} Coins";
        }

        if (_timeSpent >= _totalTime)
        {
            OnEnd("Finished");
        }
    }

    private void OnEnd(string endReason)
    {
        _passiveSessionInfoManager.setTimeSpent(this._timeSpent);
        _passiveSessionInfoManager.setAccumulatedExp(this._expAccumulated);
        _passiveSessionInfoManager.setAccumulatedCoins(this._coinsAccumulated);
        this.GetTree().ChangeSceneToFile(Paths.PassiveEndScene);
    }
}