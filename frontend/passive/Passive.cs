using Godot;
using nuscutiesapp.tools;

public partial class Passive : Control
{
    [Export]
    private Button _backToHomeButton;

    [Export] private Label _timeInfoLabel;
    [Export] private Slider _timeSlider;
    [Export] private Button _startButton;

    public override void _Ready()
    {
        _timeSlider.Value = 20;
        _timeInfoLabel.Text = $"Enter dungeon for: {_timeSlider.Value} seconds";
        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _timeSlider.ValueChanged += TimeSliderOnValueChanged;
    }

    private void TimeSliderOnValueChanged(double value)
    {
        _timeInfoLabel.Text = $"Enter dungeon for: {value} seconds";
    }

    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.Home);
    }
}