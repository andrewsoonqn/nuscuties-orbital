using Godot;
using nuscutiesapp.tools;
using System;

public partial class Passive : Control
{
    [Export]
    private Button _backToHomeButton;

    [Export] private Label _timeInfoLabel;
    [Export] private SpinBox _hoursInput;
    [Export] private SpinBox _minutesInput;
    [Export] private SpinBox _secondsInput;
    [Export] private LineEdit _sessionNameInput;
    [Export] private Label _endTimeLabel;
    [Export] private Button _startButton;

    private PassiveSessionInfoManager _passiveSessionInfoManager;
    private Timer _updateTimer;

    public override void _Ready()
    {
        _hoursInput.Value = 0;
        _minutesInput.Value = 0;
        _secondsInput.Value = 20;
        _sessionNameInput.PlaceholderText = "Enter session name...";
        _endTimeLabel.Text = "End time: --:--";

        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _hoursInput.ValueChanged += OnTimeInputChanged;
        _minutesInput.ValueChanged += OnTimeInputChanged;
        _secondsInput.ValueChanged += OnTimeInputChanged;
        _startButton.Pressed += StartButtonOnPressed;

        _passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");

        _updateTimer = new Timer();
        _updateTimer.WaitTime = 1.0f;
        _updateTimer.Timeout += UpdateEndTime;
        AddChild(_updateTimer);
        _updateTimer.Start();

        UpdateEndTime();
    }

    private void OnTimeInputChanged(double value)
    {
        UpdateEndTime();
    }

    private void UpdateEndTime()
    {
        int totalSeconds = (int)(_hoursInput.Value * 3600 + _minutesInput.Value * 60 + _secondsInput.Value);
        if (totalSeconds <= 0)
        {
            _endTimeLabel.Text = "End time: --:--";
            return;
        }

        DateTime endTime = DateTime.Now.AddSeconds(totalSeconds);
        _endTimeLabel.Text = $"End time: {endTime:HH:mm}";
    }

    private void StartButtonOnPressed()
    {
        int totalSeconds = (int)(_hoursInput.Value * 3600 + _minutesInput.Value * 60 + _secondsInput.Value);
        if (totalSeconds <= 0)
        {
            return;
        }

        _passiveSessionInfoManager.setTotalTime(totalSeconds);
        _passiveSessionInfoManager.setSessionName(_sessionNameInput.Text);
        this.GetTree().ChangeSceneToFile(Paths.PassiveOngoing);
    }

    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.Home);
    }
}