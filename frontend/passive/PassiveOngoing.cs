using Godot;
using nuscutiesapp.tools;

public partial class PassiveOngoing : Control
{
    [Export] Label _timeSpentLabel;
    [Export] Label _totalTimeLabel;
    [Export] ProgressBar _timeProgressBar;
    [Export] Label _expAccumulatedLabel;
    [Export] Button _quitButton;

    private double _totalTime;
    private double _timeSpent = 0;
    private int _expAccumulated = 0;
    private PassiveSessionInfoManager _passiveSessionInfoManager;

    public override void _Ready()
    {
        _passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");
        this._totalTime = _passiveSessionInfoManager.getTotalTime();
        _timeSpentLabel.Text = _timeSpent.ToString("F0");
        _totalTimeLabel.Text = $"out of {_totalTime:F0} seconds";
        _expAccumulatedLabel.Text = $"{_expAccumulated:F0} EXP";

        _quitButton.Pressed += () => OnEnd("Quit");
    }

    public override void _Process(double delta)
    {
        _timeSpent += delta;
        _timeSpentLabel.Text = _timeSpent.ToString("F0");
        _timeProgressBar.Ratio = _timeSpent / _totalTime;

        _expAccumulated = (int)_timeSpent / 2 * 100;
        _expAccumulatedLabel.Text = $"{_expAccumulated:F0} EXP";

        if (_timeSpent >= _totalTime)
        {
            OnEnd("Finished");
        }
    }

    private void OnEnd(string endReason)
    {
        _passiveSessionInfoManager.setTimeSpent(this._timeSpent);
        _passiveSessionInfoManager.setAccumulatedExp(this._expAccumulated);
        this.GetTree().ChangeSceneToFile(Paths.PassiveEndScene);
    }
}