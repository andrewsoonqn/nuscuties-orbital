using Godot;
using nuscutiesapp.tools;

public partial class Active : Control
{
    [Export]
    private Button _backToHomeButton;
    
    [Export]
    private Button _startGameButton;
    
    private StatsManager _statsManager;

    public override void _Ready()
    {
        _backToHomeButton.Pressed += BackToHomeButtonOnPressed;
        _statsManager = GetNode<StatsManager>("/root/StatsManager");
        _startGameButton.Pressed += StartGameButtonOnPressed;
    }

    private void StartGameButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.ActiveGame);
    }

    private void AddExpButtonOnPressed()
    {
        _statsManager.AddExp(50);
    }

    private void BackToHomeButtonOnPressed()
    {
        GetTree().ChangeSceneToFile(Paths.Home);
    }
}