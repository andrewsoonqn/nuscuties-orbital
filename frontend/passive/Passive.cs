using Godot;
using nuscutiesapp.tools;

public partial class Passive : Control
{
	[Export]
	private Button _backToHomeButton;

	[Export] private Label _timeInfoLabel;
	[Export] private Slider _timeSlider;
	[Export] private Button _startButton;

	private PassiveSessionInfoManager _passiveSessionInfoManager;
	public override void _Ready()
	{
		_timeSlider.Value = 20;
		_timeInfoLabel.Text = $"Enter dungeon for: {_timeSlider.Value} minutes";
		_backToHomeButton.Pressed += BackToHomeButtonOnPressed;
		_timeSlider.ValueChanged += TimeSliderOnValueChanged;

		_startButton.Pressed += StartButtonOnPressed;

		_passiveSessionInfoManager = this.GetNode<PassiveSessionInfoManager>("/root/PassiveSessionInfoManager");
	}

	private void StartButtonOnPressed()
	{
		_passiveSessionInfoManager.setTotalTime(_timeSlider.Value);
		this.GetTree().ChangeSceneToFile(Paths.PassiveOngoing);
	}

	private void TimeSliderOnValueChanged(double value)
	{
		_timeInfoLabel.Text = $"Enter dungeon for: {value} minutes";
	}

	private void BackToHomeButtonOnPressed()
	{
		GetTree().ChangeSceneToFile(Paths.Home);
	}
}
