using Godot;
using nuscutiesapp.tools;

public partial class PassiveOngoing : Control
{
    [Export] Label _timeSpentLabel;
    [Export] Label _totalTimeLabel;
    [Export] ProgressBar _timeProgressBar;
    [Export] Label _expAccumulatedLabel;
    [Export] Label _coinsAccumulatedLabel;
    [Export] Label _sessionNameLabel;
    [Export] Button _quitButton;
    [Export] AnimatedSprite2D _runningSprite;
    [Export] AnimationPlayer _runningSpritePlayer;
    [Export] private Control _spriteContainer;

    private double _totalTime;
    private double _timeSpent = 0;
    private int _expAccumulated = 0;
    private int _coinsAccumulated = 0;
    private PassiveSessionInfoManager _passiveSessionInfoManager;
    private PlayerInventoryManager _inventoryManager;
    private Vector2 _spriteStartPosition;
    private Vector2 _spriteEndPosition;

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

        string sessionName = _passiveSessionInfoManager.getSessionName();
        if (!string.IsNullOrEmpty(sessionName))
        {
            _sessionNameLabel.Text = $"Session: {sessionName}";
        }
        else
        {
            _sessionNameLabel.Text = "Session: Unnamed";
        }

        SetupRunningSprite();

        _quitButton.Pressed += () => OnEnd("Quit");
    }

    private void SetupRunningSprite()
    {
        if (_runningSprite != null && _spriteContainer != null)
        {
            _spriteStartPosition = new Vector2(0, _runningSprite.Position.Y);
            _spriteEndPosition = new Vector2(_spriteContainer.GetRect().Size.X, _runningSprite.Position.Y);
            _runningSpritePlayer.Play("run");
        }
    }

    public override void _Process(double delta)
    {
        _timeSpent += delta;
        _timeSpentLabel.Text = _timeSpent.ToString("F0");
        _timeProgressBar.Ratio = _timeSpent / _totalTime;

        _expAccumulated = (int)_timeSpent / 2 * 100;
        _expAccumulatedLabel.Text = $"{_expAccumulated:F0} EXP";

        _coinsAccumulated += _inventoryManager.CalculatePassiveDungeonCoinReward(delta);
        if (_coinsAccumulatedLabel != null && Mathf.RoundToInt(_timeSpent) % 5 == 0) // Update coins every 5 seconds
        {
            _coinsAccumulatedLabel.Text = $"{_coinsAccumulated:F0} Coins";
        }

        if (_timeSpent >= _totalTime)
        {
            OnEnd("Finished");
        }
    }

    private void UpdateRunningSprite(double delta)
    {
        if (_runningSprite != null)
        {
            float progress = (float)(_timeSpent / _totalTime);
            Vector2 newPosition = _spriteStartPosition.Lerp(_spriteEndPosition, progress);
            _runningSprite.Position = newPosition;

            if (progress >= 1.0f)
            {
                _runningSprite.Position = _spriteStartPosition;
            }
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