using Godot;
using nuscutiesapp.tools;

public partial class Active : Control
{
    [Export]
    private Button _backToHomeButton;

    [Export]
    private Button _startGameButton;

    private ProgressionManager _expManager;

    public override void _Ready()
    {
        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _expManager = GetNode<ProgressionManager>("/root/ProgressionManager");
        _startGameButton.Pressed += StartGameButtonOnPressed;
    }

    private void StartGameButtonOnPressed()
    {
        GetNode<AudioManager>("/root/AudioManager").PlayFightBgm();
        GetTree().ChangeSceneToFile(Paths.ActiveGame);
    }

    private void AddExpButtonOnPressed()
    {
        _expManager.AddExp(50);
    }

    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.Home);
    }
}